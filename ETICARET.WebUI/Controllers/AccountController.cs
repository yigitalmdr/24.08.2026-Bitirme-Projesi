using ETICARET.Business.Abstract;
using ETICARET.WebUI.EmailService;
using ETICARET.WebUI.Extensions;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICartService _cartService;
        private readonly IMailHelper _mailHelper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ICartService cartService, IMailHelper mailHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartService = cartService;
            _mailHelper = mailHelper;
        }

        public IActionResult Register()//get
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)//post
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = code });

                string siteUrl = $"{Request.Scheme}://{Request.Host}";
                string activeUrl = $"{siteUrl}{callbackUrl}";
                string body = $"<div style='font-family:Arial,sans-serif;max-width:560px;margin:auto;padding:32px;border:1px solid #e5e7eb;border-radius:18px'><div style='font-size:24px;font-weight:800;color:#111827;margin-bottom:24px'>üçüncübinyıl</div><h1 style='font-size:22px;color:#111827'>Hesabınızı doğrulayın</h1><p style='color:#64748b;line-height:1.6'>Alışverişe başlamak için e-posta adresinizi aşağıdaki bağlantıyla onaylayın.</p><a href='{activeUrl}' target='_blank' style='display:inline-block;margin-top:12px;background:#4f46e5;color:#fff;text-decoration:none;padding:13px 22px;border-radius:10px;font-weight:700'>Hesabımı Onayla</a></div>";
                var mailSent = await _mailHelper.SendMailAsync(body, model.Email, "üçüncübinyıl Hesap Doğrulama");
                if (!mailSent)
                {
                    await _userManager.DeleteAsync(user);
                    ModelState.AddModelError(string.Empty, "Doğrulama e-postası gönderilemedi. Lütfen e-posta adresinizi kontrol edip yeniden deneyin.");
                    return View(model);
                }
                
                TempData.Put("message", new ResultModel()
                {
                    Title = "Kayıt Başarılı",
                    Message = "Hesabınız başarıyla oluşturuldu, lütfen mailinizi doğrulayınız.",
                    Css = "success"
                });

                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }
   
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Geçersiz Token",
                    Message = "Hesap onaylama işlemi başarısız oldu",
                    Css = "danger"
                });
                return Redirect("~");//anasayfaya yönlendir

            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _cartService.InitialCart(userId);
                    TempData.Put("message", new ResultModel()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız onaylanmıştır",
                        Css = "success"
                    });
                    return RedirectToAction("Login", "Account");
                }

            }
            TempData.Put("message", new ResultModel()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanmamıştır",
                Css = "danger"
            });
            return Redirect("~");
        }
        public IActionResult Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                TempData["SessionExpiredWarning"] = "Oturum süreniz doldu veya bu işlem için giriş yapmanız gerekiyor.";
            }

            return View(
                new LoginModel()
                {
                    ReturnUrl = returnUrl
                }
            );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            ModelState.Remove("ReturnUrl");

            if (!ModelState.IsValid)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Giriş bilgileri",
                    Message = "Bilgiler Hatalıdır",
                    Css = "danger"
                });
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu email adresi ile kayıtlı bir kullanıcı bulunamadı.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı e-posta ile onaylayınız");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return LocalRedirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Hesap Kitlendi",
                    Message = "Hesabınız geçici olarak kitlenmiştir.",
                    Css = "danger"
                });
                return View(model);
            }
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");// ilk değer key ikinci değer value
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new ResultModel()
            {
                Title = "Oturum Kapatıldı",
                Message = "Hesabınız güvenli bir şekilde kapatıldı",
                Css = "success"
            });
            return Redirect("~/");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Şifremi Unuttum",
                    Message = "Lütfen email adresinizi boş bırakmayınız",
                    Css = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Şifremi Unuttum",
                    Message = "Bu email adresiyle kayıtlı bir kullanıcı bulunamadı",
                    Css = "danger"
                });
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = code });
            string siteUrl = $"{Request.Scheme}://{Request.Host}";
            string resetUrl = $"{siteUrl}{callbackUrl}";
            string body = $"<div style='font-family:Arial,sans-serif;max-width:560px;margin:auto;padding:32px;border:1px solid #e5e7eb;border-radius:18px'><div style='font-size:24px;font-weight:800;color:#111827;margin-bottom:24px'>üçüncübinyıl</div><h1 style='font-size:22px;color:#111827'>Şifrenizi yenileyin</h1><p style='color:#64748b;line-height:1.6'>Şifrenizi güvenle yenilemek için aşağıdaki bağlantıyı kullanın.</p><a href='{resetUrl}' target='_blank' style='display:inline-block;margin-top:12px;background:#4f46e5;color:#fff;text-decoration:none;padding:13px 22px;border-radius:10px;font-weight:700'>Şifremi Sıfırla</a></div>";
            var mailSent = await _mailHelper.SendMailAsync(body, email, "üçüncübinyıl Şifre Sıfırlama");
            if (!mailSent)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "E-posta Gönderilemedi",
                    Message = "Şifre sıfırlama e-postası gönderilemedi. Lütfen daha sonra yeniden deneyin.",
                    Css = "danger"
                });
                return View();
            }
            TempData.Put("message", new ResultModel()
            {
                Title = "Şifre Sıfırlama",
                Message = "Şifre sıfırlama linki e-posta adresinize gönderilmiştir.",
                Css = "success"
            });
            return RedirectToAction("Login", "Account");
        }
        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordModel { Token = token };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Şifre Sıfırlama",
                    Message = "Bu email adresiyle kayıtlı bir kullanıcı bulunamadı",
                    Css = "danger"
                });
                return RedirectToAction("Index", "Home");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Şifre Sıfırlama",
                    Message = "Şifreniz başarıyla sıfırlanmıştır.",
                    Css = "success"
                });
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(model);
            }
        }
        [Authorize]
        public async Task<IActionResult> Manage()
        {
            var user = await _userManager.GetUserAsync(User);//Giriş yapan kullanıcıyı alıyoruz
            if (user == null)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Bağlantı hatası",
                    Message = "Kullanıcı bilgileri bulunamadı tekrar deneyin",
                    Css = "danger"
                });
                return View();
            }
            var model = new AccountModel()
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email
            };
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(AccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Bağlantı hatası",
                    Message = "Kullanıcı bilgileri bulunamadı tekrar deneyin",
                    Css = "danger"
                });
                return View();
            }
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData.Put("message", new ResultModel()
                {
                    Title = "Hesap Güncelleme",
                    Message = "Hesap bilgileriniz başarıyla güncellenmiştir.",
                    Css = "success"
                });
                return RedirectToAction("Manage");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                TempData.Put("message", new ResultModel()
                {
                    Title = "Hesap Güncelleme",
                    Message = "Hesap bilgileriniz güncellenemedi. Lütfen formdaki hataları kontrol edin.",
                    Css = "danger"
                });
               
                return View(model);
            }
        }

        public IActionResult AccessDenied()
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            return View();
        }
    }
}
