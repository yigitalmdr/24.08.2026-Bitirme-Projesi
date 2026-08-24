using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

using ETICARET.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETICARET.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShopController(IProductService productService, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }
        [Route("products/{category?}")]
        public IActionResult List(string category, int page = 1, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            const int pageSize = 12;
            page = Math.Max(page, 1);
            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                (minPrice, maxPrice) = (maxPrice, minPrice);
            }
            
            var queryString = "";
            if (!string.IsNullOrEmpty(search)) queryString += $"&search={search}";
            if (!string.IsNullOrEmpty(sort)) queryString += $"&sort={sort}";
            if (minPrice.HasValue) queryString += $"&minPrice={minPrice}";
            if (maxPrice.HasValue) queryString += $"&maxPrice={maxPrice}";

            var productListModel = new ProductListModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category, search, minPrice, maxPrice),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category,
                    CurrentQueryString = queryString
                },
                Products = _productService.GetProductByCategory(category, page, pageSize, search, sort, minPrice, maxPrice)
            };
            return View(productListModel);
        }
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Product? product = _productService.GetProductDetails(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.ProductId = product.Id;
            var categories = product.ProductCategories
                .Select(i => i.Category)
                .Where(category => category is not null)
                .Cast<Category>()
                .ToList();
            var similarProducts = new List<Product>();
            if (categories.Any())
            {
                var catName = categories.First().Name;
                similarProducts = _productService.GetProductByCategory(catName, 1, 5, null, null, null, null)
                    .Where(p => p.Id != product.Id)
                    .Take(4)
                    .ToList();
            }

            var usernames = new Dictionary<string, string>();
            var adminUsers = new Dictionary<string, bool>();

            if (product.Comments != null && product.Comments.Any())
            {
                foreach (var comment in product.Comments)
                {
                    if (comment.Rating > 5)
                    {
                        comment.Rating = comment.Rating / 10.0;
                    }

                    if (!usernames.ContainsKey(comment.UserId))
                    {
                        var user = await _userManager.FindByIdAsync(comment.UserId);
                        if (user != null)
                        {
                            usernames[comment.UserId] = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.UserName ?? "Kullanıcı";
                            adminUsers[comment.UserId] = await _userManager.IsInRoleAsync(user, "admin");
                        }
                    }
                }
            }
            ViewBag.Usernames = usernames;
            ViewBag.AdminUsers = adminUsers;

            return View(new ProductDetailsModel()
            {
                Product = product,
                Categories = categories,
                Comments = product.Comments,
                SimilarProducts = similarProducts
            });
        }
    }
}
