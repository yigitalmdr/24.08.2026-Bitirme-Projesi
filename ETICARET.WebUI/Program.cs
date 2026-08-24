using ETICARET.Business.Abstract;
using ETICARET.Business.Concrete;
using ETICARET.DataAccess.Abstract;
using ETICARET.DataAccess.Concrete.EfCore;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.EmailService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // Windows Event Log is not always writable in local development environments.
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
}

var dataProtectionKeysPath = builder.Configuration["DataProtection:KeysPath"];
if (string.IsNullOrWhiteSpace(dataProtectionKeysPath))
{
    dataProtectionKeysPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtection-Keys");
}

Directory.CreateDirectory(dataProtectionKeysPath);
builder.Services.AddDataProtection()
    .SetApplicationName("ETICARET")
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysPath));

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("IdentityConnection"),
    sql => sql.EnableRetryOnFailure())
);
builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    sql =>
    {
        sql.EnableRetryOnFailure();
        sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    })
);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddErrorDescriber<TurkishIdentityErrorDescriber>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();// Email doğrulama ve şifre sıfırlama gibi işlemler için token sağlayıcılarını ekler.
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;// Şifrede en az bir rakam bulunmasını zorunlu kılar.
    options.Password.RequiredLength = 8;// Şifrenin minimum uzunluğunu 8 karakter olarak belirler.
    options.Password.RequireNonAlphanumeric = true;// Şifrede en az bir özel karakter bulunmasını zorunlu kılar.
    options.Password.RequireUppercase = true;// Şifrede en az bir büyük harf bulunmasını zorunlu kılar.
    options.Password.RequireLowercase = true;// Şifrede en az bir küçük harf bulunmasını zorunlu kılar.

    options.Lockout.MaxFailedAccessAttempts = 5;// Kullanıcının hesabının kilitlenmesi için maksimum başarısız giriş denemesi sayısını belirler.
    options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);// Hesap kilitlendikten sonra ne kadar süreyle kilitli kalacağını belirler.
    options.Lockout.AllowedForNewUsers = true;// Yeni kullanıcıların kilitlenmeye tabi olup olmayacağını belirler.

    options.User.RequireUniqueEmail = true;// Kullanıcıların benzersiz e-posta adreslerine sahip olmasını zorunlu kılar.
    options.SignIn.RequireConfirmedEmail = true;// Kullanıcıların e-posta doğrulaması yapmadan giriş yapmalarını engeller.
    options.SignIn.RequireConfirmedPhoneNumber = false;// Kullanıcıların telefon numarası doğrulaması yapmadan giriş yapmalarını engeller.

});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";// Kullanıcı giriş sayfasının yolunu belirler.
    options.LogoutPath = "/Account/Logout";// Kullanıcı çıkış sayfasının yolunu belirler.
    options.AccessDeniedPath = "/Account/AccessDenied";// Erişim reddedildiğinde yönlendirilecek sayfanın yolunu belirler.
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);// Oturumun geçerlilik süresini belirler.
    options.SlidingExpiration = true;// Oturum süresinin her istekte yenilenip yenilenmeyeceğini belirler.
    options.Cookie = new CookieBuilder()
    {
        HttpOnly = true,// Çerezlerin yalnızca HTTP istekleriyle erişilebilir olmasını sağlar.
        Name= ".ETICARET.Security.Cookie",// Çerezin adını belirler.
        SameSite = SameSiteMode.Lax,// Çerezin SameSite özelliğini belirler. Iyzico geri dönüşünde oturumun düşmemesi için Lax yapılmalıdır.

    };
});

//Data Access DI katmanı
builder.Services.AddScoped<IProductDal, EfCoreProductDal>();
builder.Services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
builder.Services.AddScoped<IOrderDal, EfCoreOrderDal>();
builder.Services.AddScoped<ICommentDal, EfCoreCommentDal>();
builder.Services.AddScoped<ICartDal, EfCoreCartDal>();
builder.Services.AddScoped<IFavoriteDal, EfCoreFavoriteDal>();

//Bussiness DI katmanı
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<ICartService, CartManager>();
builder.Services.AddScoped<IFavoriteService, FavoriteManager>();

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, field) => $"'{value}' değeri {field} alanı için geçersizdir.");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(field => $"{field} alanı zorunludur.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Gerekli bir değer eksik.");
    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(field => $"{field} alanı için geçersiz bir değer girildi.");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(value => $"'{value}' değeri geçersizdir.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(field => $"{field} alanı sayı olmalıdır.");
    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(value => $"'{value}' değeri geçersizdir.");
    options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Girilen değer geçersizdir.");
    options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "Girilen değer sayı olmalıdır.");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();//wwwroot klasöründeki statik dosyaları sunmak için kullanılır.

app.UseRouting();
app.UseAuthentication();//Kullanıcı kimlik doğrulamasını etkinleştirir. Bu, kullanıcıların sisteme giriş yapmasını ve kimliklerini doğrulamasını sağlar.
app.UseAuthorization();//Kullanıcı yetkilendirmesini etkinleştirir. Bu, kullanıcıların belirli kaynaklara veya işlemlere erişim izinlerini kontrol eder.

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services= scope.ServiceProvider;//Scope oluşturuldu ve servis sağlayıcı alındı.
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate(); // Tablo ve veritabanı yoksa oluşturur
    SeedDatabase.Seed(context);

    var identityContext = services.GetRequiredService<ApplicationIdentityDbContext>();
    identityContext.Database.Migrate(); // Identity tablolarını ve veritabanını oluşturur

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();//UserManager servisi alındı.
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();//RoleManager servisi alındı.

    await SeedIdentity.Seed(userManager, roleManager, app.Configuration);

    var adminEmail = app.Configuration["Data:AdminUser:email"];
    if (!string.IsNullOrWhiteSpace(adminEmail))
    {
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is not null)
        {
            var cartService = services.GetRequiredService<ICartService>();
            if (cartService.GetCartByUserId(adminUser.Id) is null)
            {
                await cartService.InitialCartAsync(adminUser.Id);
            }
        }
    }
}

app.Run();
