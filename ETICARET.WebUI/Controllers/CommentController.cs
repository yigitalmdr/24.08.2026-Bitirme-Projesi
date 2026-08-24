using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Extensions;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ICommentService commentService, UserManager<ApplicationUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            if (!ModelState.IsValid || model.ProductId <= 0)
            {
                TempData.Put("message", new ResultModel
                {
                    Title = "Yorum Gönderilemedi",
                    Message = ModelState.Values.SelectMany(value => value.Errors).FirstOrDefault()?.ErrorMessage ?? "Yorum bilgileri geçersizdir.",
                    Css = "danger"
                });
                return RedirectToAction("Details", "Shop", new { id = model.ProductId });
            }

            var rating = model.Rating > 5 ? model.Rating / 10.0 : model.Rating;
            rating = Math.Clamp(rating, 0, 5);

            var comment = new Comment
            {
                Text = model.Text,
                ProductId = model.ProductId,
                UserId = user.Id,
                CreateOn = DateTime.Now,
                Rating = rating
            };
            _commentService.Create(comment);
            TempData.Put("message", new ResultModel
            {
                Title = "Yorum Gönderildi",
                Message = "Değerlendirmeniz başarıyla kaydedildi.",
                Css = "success"
            });
            return RedirectToAction("Details", "Shop", new { id = model.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }

            var comment = _commentService.GetById(id);
            if (comment != null && (comment.UserId == user.Id || User.IsInRole("admin")))
            {
                _commentService.Delete(comment);
            }
            return RedirectToAction("Details", "Shop", new { id = productId });
        }
    }
}
