using Microsoft.AspNetCore.Identity;

namespace ETICARET.WebUI.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public required string FullName { get; set; }
    }
}
