using MailKit.Net.Smtp;
using MimeKit;
using Polly.Retry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;

namespace AssetHub.BackgroundJobs
{
    public class MailKitEmailSender
    {
        private readonly AsyncRetryPolicy _retryPolicy;

        public MailKitEmailSender()
        {
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds}s due to: {exception.Message}");
                    });
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse("youremail@gmail.com"));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            await _retryPolicy.ExecuteAsync(async () =>
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("aryankhatiwoda9@gmail.com", "uopq twss wpus dyyy");
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }); 
        }
    }
}
