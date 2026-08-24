using System.ComponentModel.DataAnnotations;
using ETICARET.WebUI.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yorum alanı boş bırakılamaz.")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Yorum 2 ile 1000 karakter arasında olmalıdır.")]
        public string Text { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Ürün bilgisi geçersizdir.")]
        public int ProductId { get; set; }

        [Range(0.5, 5, ErrorMessage = "Puan 0,5 ile 5 arasında olmalıdır.")]
        [ModelBinder(BinderType = typeof(FlexibleDoubleModelBinder))]
        public double Rating { get; set; } = 5;
    }
}
