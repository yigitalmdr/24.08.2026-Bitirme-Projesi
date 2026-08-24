using ETICARET.Entities;
using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200,MinimumLength =5,ErrorMessage ="Ürün Adı min 5 max 200 karakter olmalıdır")]
        public string Name { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Ürün Açıklaması min 5 max 2000 karakter olmalıdır")]
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat geçerli bir değer olmalıdır.Lütfen pozitif bir değer giriniz...")]
        public decimal Price { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stok adedi 0 veya daha büyük olmalıdır.")]
        public int Stock { get; set; } = 100;
        public List<Image>? Images { get; set; }
        public List<Category>? SelectedCategories { get; set; }
        public string? CategoryId { get; set; }
        public ProductModel()
        {
            Images = new List<Image>();
        }
    }
}
