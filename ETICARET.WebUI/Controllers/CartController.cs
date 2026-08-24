using ETICARET.Business.Abstract;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;

namespace ETICARET.WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager, IOrderService orderService, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IProductService productService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _orderService = orderService;
            _configuration = configuration;
            _signInManager = signInManager;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            var cart = _cartService.GetCartByUserId(user.Id);
            if (cart is null)
            {
                await _cartService.InitialCartAsync(user.Id);
                cart = await _cartService.GetCartByUserIdAsync(user.Id);
            }

            if (cart is null)
            {
                return Problem("Kullanıcı sepeti oluşturulamadı.");
            }

            var model = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (decimal)i.Product.Price,
                    ImageUrl = i.Product.Images.FirstOrDefault()?.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            if (TempData["DiscountCode"] != null)
            {
                TempData.Keep("DiscountCode");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            if (productId <= 0 || quantity <= 0)
            {
                TempData["CartError"] = "Ürün ve adet bilgisi geçersiz.";
                return RedirectToAction("Index");
            }

            var product = _productService.GetById(productId);
            if (product is null)
            {
                return NotFound();
            }

            var cart = _cartService.GetCartByUserId(user.Id);
            var existingQuantity = cart?.CartItems.FirstOrDefault(item => item.ProductId == productId)?.Quantity ?? 0;
            if (product.Stock <= 0 || existingQuantity + quantity > product.Stock)
            {
                TempData["CartError"] = "İstenen adet stok miktarını aşıyor.";
                return RedirectToAction("Details", "Shop", new { id = productId });
            }

            _cartService.AddToCart(user.Id, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            _cartService.DeleteFromCart(user.Id, productId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            var orders = await _orderService.GetOrdersAsync(user.Id);

            var model = new List<OrderListModel>();

            foreach (var order in orders)
            {
                var orderModel = new OrderListModel()
                {
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    OrderState = order.OrderState,
                    PaymentTypes = order.PaymentTypes,
                    FirstName = order.FirstName,
                    LastName = order.LastName,
                    Address = order.Address,
                    City = order.City,
                    Phone = order.Phone,
                    Email = order.Email,
                    OrderNote = order.OrderNote,
                    OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                    {
                        OrderItemId = i.Id,
                        Name = i.Product.Name,
                        Price = (decimal)i.Price,
                        Quantity = i.Quantity,
                        ImageUrl = i.Product.Images.FirstOrDefault()?.ImageUrl,
                        ProductId = i.ProductId
                    }).ToList()
                };

                model.Add(orderModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyDiscount(string discountCode)
        {
            if (!string.IsNullOrEmpty(discountCode) && discountCode.Trim().ToUpper() == "UCUNCUBINYIL")
            {
                var user = await _userManager.GetUserAsync(User);
                if (user is null)
                {
                    return Challenge();
                }

                var cart = _cartService.GetCartByUserId(user.Id);
                if (cart is null)
                {
                    TempData["DiscountError"] = "Sepet bulunamadı.";
                    return RedirectToAction("Index");
                }
                decimal total = (decimal)(cart.CartItems?.Sum(c => c.Product.Price * c.Quantity) ?? 0);

                if (total <= 1000)
                {
                    TempData["DiscountCode"] = "UCUNCUBINYIL";
                    TempData["DiscountMessage"] = "Kupon uygulandı: %20 İndirim!";
                }
                else
                {
                    TempData["DiscountError"] = "Bu kupon yalnızca 1000 TL ve altındaki sepet tutarlarında geçerlidir.";
                }
            }
            else
            {
                TempData["DiscountError"] = "Geçersiz kupon kodu.";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            var cart = _cartService.GetCartByUserId(user.Id);
            if (cart is null)
            {
                await _cartService.InitialCartAsync(user.Id);
                cart = await _cartService.GetCartByUserIdAsync(user.Id);
            }

            if (cart is null)
            {
                return Problem("Kullanıcı sepeti oluşturulamadı.");
            }

            var orderModel = new OrderModel();
            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (decimal)i.Product.Price,
                    ImageUrl = i.Product.Images.FirstOrDefault()?.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            if (TempData["DiscountCode"] != null)
            {
                orderModel.DiscountCode = TempData["DiscountCode"].ToString();
                TempData.Keep("DiscountCode");
            }

            if (!string.IsNullOrEmpty(user.FullName))
            {
                var names = user.FullName.Split(' ');
                orderModel.FirstName = names[0];
                orderModel.LastName = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "";
            }
            orderModel.Email = user.Email;

            return View(orderModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            var cart = _cartService.GetCartByUserId(user.Id);
            
            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index");
            }

            model.CartModel = new CartModel
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (decimal)i.Product.Price,
                    ImageUrl = i.Product.Images.FirstOrDefault()?.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (cart.CartItems.Any(item => item.Quantity <= 0 || item.Quantity > item.Product.Stock))
            {
                ModelState.AddModelError(string.Empty, "Sepetteki ürünlerden birinin stok miktarı değişti. Lütfen sepetinizi kontrol edin.");
                return View(model);
            }

            // Calculate Totals
            decimal totalPrice = cart.CartItems.Sum(i => (decimal)i.Product.Price * i.Quantity);
            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(model.DiscountCode) && model.DiscountCode.ToUpper() == "UCUNCUBINYIL")
            {
                if (totalPrice <= 1000)
                {
                    discountAmount = totalPrice * 0.2m;
                }
                else
                {
                    discountAmount = 0;
                }
            }
            decimal finalPrice = totalPrice - discountAmount;

            // Iyzico Payment Options
            Options options = new Options();
            options.ApiKey = _configuration["IyzicoOptions:ApiKey"];
            options.SecretKey = _configuration["IyzicoOptions:SecretKey"];
            options.BaseUrl = _configuration["IyzicoOptions:BaseUrl"];

            // Iyzico Payment Request (3D Secure)
            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Price = totalPrice.ToString("0.00").Replace(",", ".");
            request.PaidPrice = finalPrice.ToString("0.00").Replace(",", ".");
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = cart.Id.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = Url.Action("IyzicoCallback", "Cart", null, Request.Scheme);

            // Payment Card
            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber?.Replace(" ", "");
            paymentCard.ExpireMonth = model.ExprationMonth;
            paymentCard.ExpireYear = model.ExprationYear;
            paymentCard.Cvc = model.CVV;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            // Buyer
            Buyer buyer = new Buyer();
            buyer.Id = user.Id;
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = model.Phone;
            buyer.Email = model.Email;
            buyer.IdentityNumber = "11111111111"; // Required by iyzico, placeholder for testing
            buyer.LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            buyer.RegistrationAddress = model.Address;
            buyer.Ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "85.34.78.112";
            buyer.City = model.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            // Address
            Address billingAddress = new Address();
            billingAddress.ContactName = $"{model.FirstName} {model.LastName}";
            billingAddress.City = model.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = model.Address;
            billingAddress.ZipCode = "34732";
            request.BillingAddress = billingAddress;
            request.ShippingAddress = billingAddress;

            // Basket Items
            List<BasketItem> basketItems = new List<BasketItem>();
            foreach (var item in cart.CartItems)
            {
                BasketItem basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Product.Name;
                basketItem.Category1 = "Collectibles"; // Placeholder
                basketItem.Category2 = "Accessories"; // Placeholder
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                
                decimal itemTotal = (decimal)item.Product.Price * item.Quantity;
                basketItem.Price = itemTotal.ToString("0.00").Replace(",", ".");
                
                basketItems.Add(basketItem);
            }
            request.BasketItems = basketItems;

            // Execute 3D Secure Initialization
            ThreedsInitialize initialize = await ThreedsInitialize.Create(request, options);

            if (initialize.Status == "success")
            {
                // Create Pending Order
                var order = new ETICARET.Entities.Order()
                {
                    OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..25].ToUpperInvariant(),
                    OrderState = ETICARET.Entities.EnumOrderState.unpaid, // Will be completed in callback
                    PaymentTypes = ETICARET.Entities.EnumPaymentTypes.CreditCard,
                    ConversionId = request.ConversationId,
                    OrderDate = DateTime.Now,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserId = user.Id,
                    Address = model.Address,
                    City = model.City,
                    Phone = model.Phone,
                    Email = model.Email,
                    OrderNote = model.OrderNote
                };

                foreach (var item in cart.CartItems)
                {
                    decimal currentPrice = (decimal)item.Product.Price;
                    if (discountAmount > 0)
                    {
                        currentPrice = currentPrice * 0.8m;
                    }

                    order.OrderItems.Add(new ETICARET.Entities.OrderItem()
                    {
                        Price = currentPrice,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId
                    });
                }

                _orderService.Create(order);

                // Render the 3D Secure HTML form from Iyzico
                return Content(initialize.HtmlContent, "text/html");
            }
            else
            {
                // Initialization failed
                TempData["PaymentError"] = initialize.ErrorMessage;
                
                // Keep model state for view
                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = (decimal)i.Product.Price,
                        ImageUrl = i.Product.Images.FirstOrDefault()?.ImageUrl,
                        Quantity = i.Quantity
                    }).ToList()
                };
                
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> IyzicoCallback()
        {
            string paymentId = Request.Form["paymentId"];
            string conversationData = Request.Form["conversationData"];
            string conversationId = Request.Form["conversationId"];
            string status = Request.Form["status"];

            if (status != "success")
            {
                TempData["PaymentError"] = "3D Secure işlemi başarısız oldu veya iptal edildi.";
                return Content("<script>window.location.href = '/Cart/Index';</script>", "text/html"); 
            }

            // Iyzico Payment Options
            Options options = new Options();
            options.ApiKey = _configuration["IyzicoOptions:ApiKey"];
            options.SecretKey = _configuration["IyzicoOptions:SecretKey"];
            options.BaseUrl = _configuration["IyzicoOptions:BaseUrl"];

            CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = conversationId;
            request.PaymentId = paymentId;
            request.ConversationData = conversationData;

            ThreedsPayment threedsPayment = await ThreedsPayment.Create(request, options);

            if (threedsPayment.Status == "success")
            {
                var order = _orderService.GetOrderByConversionId(conversationId);
                if (order != null)
                {
                    order.OrderState = ETICARET.Entities.EnumOrderState.completed;
                    order.PaymentId = threedsPayment.PaymentId;
                    _orderService.Update(order);
                    
                    var cart = _cartService.GetCartByUserId(order.UserId);
                    if (cart != null)
                    {
                        _cartService.ClearCart(cart.Id.ToString());
                    }

                    // Güvenlik: Çapraz site POST isteğinden dolayı düşen oturumu yeniden aç
                    var user = await _userManager.FindByIdAsync(order.UserId);
                    if (user != null)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: true);
                    }
                }
                
                TempData["PaymentSuccess"] = "İşlem onaylandı. Ödemeniz başarıyla alındı!";
                TempData.Remove("DiscountCode");
                return Content("<script>window.location.href = '/Cart/GetOrders';</script>", "text/html");
            }
            else
            {
                TempData["PaymentError"] = threedsPayment.ErrorMessage;
                return Content("<script>window.location.href = '/Cart/Index';</script>", "text/html");
            }
        }
    }
}
