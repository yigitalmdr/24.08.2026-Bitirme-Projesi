using System.Diagnostics;
using System.Text.Json;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using MailKitSmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ETICARET.WebUI.EmailService
{
    public sealed class MailSettings
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public string FromName { get; set; } = "üçüncübinyıl";
    }

    public interface IMailHelper
    {
        Task<bool> SendMailAsync(string body, string to, string subject, bool isHtml = true, CancellationToken cancellationToken = default);
    }

    public sealed class MailHelper(IOptions<MailSettings> options, ILogger<MailHelper> logger, IWebHostEnvironment environment) : IMailHelper
    {
        private readonly MailSettings _settings = options.Value;

        public async Task<bool> SendMailAsync(string body, string to, string subject, bool isHtml = true, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_settings.UserName) ||
                string.IsNullOrWhiteSpace(_settings.Password) ||
                string.IsNullOrWhiteSpace(_settings.FromAddress))
            {
                logger.LogError("SMTP ayarları eksik olduğu için e-posta gönderilemedi.");
                return false;
            }

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;
                message.Body = new TextPart(isHtml ? "html" : "plain") { Text = body };

                // Google uygulama şifreleri ekranda boşluklu gösterilebilir; SMTP'ye boşluksuz gönderilmelidir.
                var normalizedPassword = string.Concat(_settings.Password.Where(character => !char.IsWhiteSpace(character)));
                var socketOptions = !_settings.EnableSsl
                    ? SecureSocketOptions.None
                    : _settings.Port == 465
                        ? SecureSocketOptions.SslOnConnect
                        : SecureSocketOptions.StartTls;

                using var smtp = new MailKitSmtpClient { Timeout = 20000 };
                await smtp.ConnectAsync(_settings.Host, _settings.Port, socketOptions, cancellationToken);
                await smtp.AuthenticateAsync(_settings.UserName, normalizedPassword, cancellationToken);
                await smtp.SendAsync(message, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);
                return true;

            }
            catch (SslHandshakeException ex)
            {
                logger.LogWarning(ex, ".NET TLS bağlantısı kurulamadı; yerel OpenSSL tabanlı SMTP yedeği deneniyor.");
                return await SendWithLocalFallbackAsync(body, to, subject, isHtml, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "E-posta {Recipient} adresine gönderilemedi.", to);
                return false;
            }
        }

        private async Task<bool> SendWithLocalFallbackAsync(string body, string to, string subject, bool isHtml, CancellationToken cancellationToken)
        {
            var scriptPath = Path.Combine(environment.ContentRootPath, "EmailService", "smtp_fallback.py");
            if (!File.Exists(scriptPath))
            {
                logger.LogError("SMTP yedek betiği bulunamadı: {ScriptPath}", scriptPath);
                return false;
            }

            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var configuredPython = Environment.GetEnvironmentVariable("ETICARET_PYTHON");
            var candidates = new[]
            {
                configuredPython,
                Path.Combine(userProfile, ".cache", "codex-runtimes", "codex-primary-runtime", "dependencies", "python", "python.exe"),
                "python3",
                "python"
            }.Where(candidate => !string.IsNullOrWhiteSpace(candidate)).Distinct(StringComparer.OrdinalIgnoreCase);

            var normalizedPassword = string.Concat(_settings.Password.Where(character => !char.IsWhiteSpace(character)));
            var payload = JsonSerializer.Serialize(new
            {
                host = _settings.Host,
                port = _settings.Port,
                enableSsl = _settings.EnableSsl,
                username = _settings.UserName,
                password = normalizedPassword,
                fromAddress = _settings.FromAddress,
                fromName = _settings.FromName,
                recipient = to,
                subject,
                body,
                isHtml
            });

            foreach (var python in candidates)
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = python!,
                        Arguments = $"\"{scriptPath}\"",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using var process = Process.Start(startInfo);
                    if (process is null)
                    {
                        continue;
                    }

                    var standardOutputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
                    var standardErrorTask = process.StandardError.ReadToEndAsync(cancellationToken);
                    await process.StandardInput.WriteAsync(payload.AsMemory(), cancellationToken);
                    process.StandardInput.Close();
                    await process.WaitForExitAsync(cancellationToken);
                    var standardOutput = await standardOutputTask;
                    var standardError = await standardErrorTask;

                    if (process.ExitCode == 0 && standardOutput.Trim().Equals("OK", StringComparison.Ordinal))
                    {
                        logger.LogInformation("E-posta {Recipient} adresine yerel SMTP yedeğiyle gönderildi.", to);
                        return true;
                    }

                    logger.LogWarning("Yerel SMTP yedeği başarısız oldu ({ExitCode}): {Error}", process.ExitCode, standardError.Trim());
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    logger.LogDebug(ex, "{PythonExecutable} SMTP yedeği için çalıştırılamadı.", python);
                }
            }

            logger.LogError("E-posta {Recipient} adresine hiçbir SMTP taşımasıyla gönderilemedi.", to);
            return false;
        }
    }
}
