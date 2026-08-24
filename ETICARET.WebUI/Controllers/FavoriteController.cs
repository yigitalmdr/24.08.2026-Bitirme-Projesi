using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ETICARET.WebUI.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;

        public FavoriteController(IFavoriteService favoriteService, UserManager<ApplicationUser> userManager, IProductService productService)
        {
            _favoriteService = favoriteService;
            _userManager = userManager;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var favorites = _favoriteService.GetFavoritesByUserId(user.Id);
            return View(favorites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });
            if (productId <= 0 || _productService.GetById(productId) is null)
            {
                return NotFound();
            }

            var existingFavorite = _favoriteService.GetFavorite(user.Id, productId);
            if (existingFavorite != null)
            {
                // Zaten favorilerde, çıkaralım
                _favoriteService.Delete(existingFavorite);
                var favoriteCount = _favoriteService.GetFavoritesByUserId(user.Id).Count;
                return Json(new { success = true, isFavorite = false, favoriteCount, message = "Ürün favorilerinizden çıkarıldı." });
            }
            else
            {
                // Favorilere ekleyelim
                var favorite = new Favorite()
                {
                    UserId = user.Id,
                    ProductId = productId
                };
                _favoriteService.Create(favorite);
                var favoriteCount = _favoriteService.GetFavoritesByUserId(user.Id).Count;
                return Json(new { success = true, isFavorite = true, favoriteCount, message = "Ürün favorilerinize eklendi." });
            }
        }
    }
}
