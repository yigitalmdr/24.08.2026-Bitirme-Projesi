# ETICARET

ASP.NET Core MVC, Entity Framework Core ve SQL Server ile geliştirilmiş katmanlı bir e-ticaret uygulamasıdır. Kullanıcı yönetimi, sepet, favoriler, yorumlar, sipariş takibi, yönetim paneli ve iyzico 3D Secure ödeme entegrasyonu içerir.

## Kullanılan teknolojiler

- .NET 10 ve ASP.NET Core MVC
- Entity Framework Core ve SQL Server
- ASP.NET Core Identity
- iyzico 3D Secure
- MailKit
- Bootstrap ve jQuery

## Yerel kurulum

1. [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ve SQL Server kurulu olmalıdır.
2. `ETICARET.WebUI/appsettings.Example.json` dosyasını `ETICARET.WebUI/appsettings.json` adıyla kopyalayıp bağlantı ve servis bilgilerini doldurun.
3. Bağımlılıkları yükleyip uygulamayı başlatın:

```powershell
dotnet restore ETICARET.slnx
dotnet run --project ETICARET.WebUI/ETICARET.WebUI.csproj
```

Veritabanı migration'ları uygulama başlarken otomatik olarak çalıştırılır. Gerçek parola, API anahtarı ve bağlantı bilgilerini repoya eklemeyin; yerel ayar dosyası veya ortam değişkenleri kullanın.
