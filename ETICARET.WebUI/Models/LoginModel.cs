using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public string? ReturnUrl { get; set; }

    }
}
