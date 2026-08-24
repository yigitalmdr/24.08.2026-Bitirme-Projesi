using Microsoft.AspNetCore.Identity;

namespace ETICARET.WebUI.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var username = configuration["Data:AdminUser:username"];
            var password = configuration["Data:AdminUser:password"];
            var email = configuration["Data:AdminUser:email"];
            var fullName = configuration["Data:AdminUser:fullName"];
            var role = configuration["Data:AdminUser:role"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    FullName = fullName ?? "Yiğit Alemdar",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, password!);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }

            var userChanged = false;
            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                userChanged = true;
            }

            if (!string.IsNullOrWhiteSpace(fullName) && user.FullName != fullName)
            {
                user.FullName = fullName;
                userChanged = true;
            }

            if (userChanged)
            {
                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", updateResult.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                var addToRoleResult = await userManager.AddToRoleAsync(user, role);
                if (!addToRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", addToRoleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
