using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ETICARET.WebUI.Extensions;

namespace ETICARET.WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ETICARET.WebUI.Identity.ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public AdminController(IProductService productService, ICategoryService categoryService, ICommentService commentService, Microsoft.AspNetCore.Identity.UserManager<ETICARET.WebUI.Identity.ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _commentService = commentService;
            _userManager = userManager;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAll();
            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalCategories = _categoryService.GetAll().Count;
            ViewBag.TotalComments = _commentService.GetAll().Count;
            ViewBag.TotalUsers = _userManager.Users.Count();

            ViewBag.CriticalStockCount = products.Count(p => p.Stock > 0 && p.Stock <= 10);
            ViewBag.OutOfStockCount = products.Count(p => p.Stock == 0);

            return View();
        }

        public IActionResult UserList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null && currentUser.Id == user.Id)
                {
                    TempData.Put("message", new ResultModel { Title = "Hata", Message = "Kendi hesabınızı silemezsiniz.", Css = "danger" });
                    return RedirectToAction("UserList");
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData.Put("message", new ResultModel { Title = "Başarılı", Message = "Kullanıcı hesabı başarıyla silindi.", Css = "success" });
                }
                else
                {
                    TempData.Put("message", new ResultModel { Title = "Hata", Message = "Kullanıcı hesabı silinirken bir hata oluştu.", Css = "danger" });
                }
            }
            return RedirectToAction("UserList");
        }

        public IActionResult CommentList()
        {
            // For now, let's fetch all comments. We'll join them in the view or here.
            var comments = _commentService.GetAll();
            var products = _productService.GetAll();
            var users = _userManager.Users.ToList();

            var commentViewModels = comments.Select(c => new CommentViewModel
            {
                Id = c.Id,
                Text = c.Text,
                CreateOn = c.CreateOn,
                Rating = c.Rating,
                ProductName = products.FirstOrDefault(p => p.Id == c.ProductId)?.Name ?? "Bilinmiyor",
                UserName = users.FirstOrDefault(u => u.Id == c.UserId)?.FullName ?? "Bilinmiyor",
                ProductId = c.ProductId
            }).OrderByDescending(c => c.CreateOn).ToList();

            return View(commentViewModels);
        }

        [HttpPost]
        public IActionResult DeleteComment(int id)
        {
            var comment = _commentService.GetById(id);
            if (comment != null)
            {
                _commentService.Delete(comment);
                TempData.Put("message", new ResultModel { Title = "Başarılı", Message = "Yorum başarıyla silindi.", Css = "success" });
            }
            return RedirectToAction("CommentList");
        }

        public IActionResult ProductList()
        {
            var model = new ProductListModel()
            {
                Products = _productService.GetAll()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _categoryService.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel model, List<IFormFile> files, int[] categoryIds)
        {
            var uploadedFiles = files?.Where(file => file.Length > 0).ToList() ?? [];
            var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg", ".jpeg", ".png", ".webp", ".gif"
            };

            foreach (var file in uploadedFiles)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("files", $"{file.FileName} desteklenen bir görsel türü değil.");
                }
                if (file.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("files", $"{file.FileName} 5 MB sınırını aşıyor.");
                }
            }

            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Stock = model.Stock
                };

                if (uploadedFiles.Count > 0)
                {
                    var imageDirectory = Path.Combine(_environment.WebRootPath, "img");
                    Directory.CreateDirectory(imageDirectory);

                    foreach (var file in uploadedFiles)
                    {
                        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        var randomName = $"{Guid.NewGuid():N}{extension}";
                        var path = Path.Combine(imageDirectory, randomName);

                        await using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                        {
                            await file.CopyToAsync(stream);
                        }

                        entity.Images.Add(new Image { ImageUrl = randomName });
                    }
                }

                if (categoryIds != null && categoryIds.Length > 0)
                {
                    entity.ProductCategories = categoryIds.Select(categoryId => new ProductCategory()
                    {
                        CategoryId = categoryId
                    }).ToList();
                }

                _productService.Create(entity);
                TempData.Put("message", new ResultModel { Title = "Success", Message = "Product created successfully", Css = "success" });
                return RedirectToAction("ProductList");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var entity = _productService.GetProductDetails(id);
            ViewBag.Categories = _categoryService.GetAll();
            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Stock = entity.Stock,
                Images = entity.Images,
                SelectedCategories = entity.ProductCategories?.Select(i => i.Category).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductModel model, int[] categoryIds)
        {
            if (ModelState.IsValid)
            {
                var entity = _productService.GetById(model.Id);
                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;
                entity.Stock = model.Stock;

                _productService.Update(entity, categoryIds);
                TempData.Put("message", new ResultModel { Title = "Success", Message = "Product updated successfully", Css = "success" });
                return RedirectToAction("ProductList");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var entity = _productService.GetById(id);
            if (entity != null)
            {
                _productService.Delete(entity);
                TempData.Put("message", new ResultModel { Title = "Success", Message = "Product deleted successfully", Css = "danger" });
            }
            return RedirectToAction("ProductList");
        }
        
        public IActionResult CategoryList()
        {
            var model = new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name
                };

                _categoryService.Create(entity);
                TempData.Put("message", new ResultModel { Title = "Success", Message = "Category created successfully", Css = "success" });
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var entity = _categoryService.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.Id);
                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;

                _categoryService.Update(entity);
                TempData.Put("message", new ResultModel { Title = "Success", Message = "Category updated successfully", Css = "success" });
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }
        
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var entity = _categoryService.GetById(id);
            if (entity != null)
            {
                _categoryService.Delete(entity);
                TempData.Put("message", new ResultModel { Title = "Success", Message = "Category deleted successfully", Css = "danger" });
            }
            return RedirectToAction("CategoryList");
        }
    }
}
