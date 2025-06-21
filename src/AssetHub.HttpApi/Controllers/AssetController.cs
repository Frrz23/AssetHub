using AssetHub.Asset;
using AssetHub.Dashboard;
using AssetHub.Entities.Asset;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Emailing;

namespace AssetHub.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetController : AbpController
    {
        private readonly IAssetAppService _assetAppService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AssetController> _logger;


        public AssetController(IAssetAppService assetAppService, IEmailSender emailSender, ILogger<AssetController> logger)
        {
            _assetAppService = assetAppService;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var assets = await _assetAppService.GetListAsync();
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(Guid id)
        {
            var asset = await _assetAppService.GetAsync(id);
            return Ok(asset);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetDto input)
        {
            var asset = await _assetAppService.CreateAsync(input);
            return CreatedAtAction(nameof(GetAssetById), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(Guid id, [FromBody] CreateAssetDto input)
        {
            var asset = await _assetAppService.UpdateAsync(id, input);
            return Ok(asset);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            await _assetAppService.DeleteAsync(id);
            return NoContent();
        }
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateAsset(Guid id)
        {
            await _assetAppService.DeactivateAsync(id);
            return NoContent();
        }
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveAsset(Guid id)
        {
            await _assetAppService.ApproveAsync(id);
            return NoContent();
        }

        [HttpGet("download-template")]
        public async Task<IActionResult> DownloadTemplate()
        {
            var file = await _assetAppService.DownloadTemplateAsync();
            return File(file.Content, file.ContentType, file.FileName);
        }


        [HttpGet("export")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public async Task<IActionResult> ExportToExcel()
        {
            var fileDto = await _assetAppService.ExportToExcelAsync();

            // This returns a FileContentResult so Swagger/browser will download it
            return File(
                fileDto.Content,
                fileDto.ContentType,
                fileDto.FileName
            );
        }
        [HttpPost("import-excel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportFromExcelAsync([FromForm] ImportExcelInputDto input)
        {
            var file = input.File;

            if (file == null || file.Length == 0)
            {
                throw new UserFriendlyException("Invalid file.");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            await _assetAppService.ImportFromExcelAsync(stream.ToArray(), file.FileName);
            return NoContent();
        }
        [HttpGet("dashboard")]
        public async Task<AssetDashboardDto> GetDashboardStatsAsync()
        {
            return await _assetAppService.GetDashboardStatsAsync();
        }
        [HttpPost("send-test-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendTestEmail()
        {
            try
            {
                _logger.LogInformation("Starting email send process...");

                await _emailSender.SendAsync(
                    "aryankhatiwoda9@gmail.com",
                    "Test from AssetHub - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    "<html><body><h2>Test Email</h2><p>This is a test email from AssetHub.</p><p>Sent at: " + DateTime.Now + "</p></body></html>",
                    true
                );

                _logger.LogInformation("Email send method completed successfully");
                return Ok($"Email sent successfully at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send test email: {Message}", ex.Message);
                return BadRequest($"Failed to send email: {ex.Message}");
            }
        }

        [HttpPost("send-test-email-debug")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendTestEmailDebug()
        {
            try
            {
                _logger.LogInformation("Starting direct MailKit email send...");

                using var client = new MailKit.Net.Smtp.SmtpClient();

                // Enable detailed logging
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                _logger.LogInformation("Connecting to smtp.gmail.com:587...");
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                _logger.LogInformation("Connected successfully. Server capabilities: {Capabilities}", string.Join(", ", client.Capabilities));

                _logger.LogInformation("Authenticating with Gmail...");
                await client.AuthenticateAsync("aryankhatiwoda9@gmail.com", "uopq twss wpus dyyy"); // Replace with your actual credentials
                _logger.LogInformation("Authentication successful");

                var message = new MimeKit.MimeMessage();
                message.From.Add(new MimeKit.MailboxAddress("AssetHub System", "your@gmail.com")); // Replace with your email
                message.To.Add(new MimeKit.MailboxAddress("Test Recipient", "aryankhatiwoda9@gmail.com"));
                message.Subject = "Direct MailKit Test - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var bodyBuilder = new MimeKit.BodyBuilder();
                bodyBuilder.HtmlBody = $@"
            <html>
            <body>
                <h2>Direct MailKit Test Email</h2>
                <p>This email was sent directly using MailKit (bypassing ABP's email sender).</p>
                <p><strong>Sent at:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
                <p><strong>From:</strong> AssetHub System</p>
                <p><strong>Test ID:</strong> {Guid.NewGuid()}</p>
            </body>
            </html>";

                message.Body = bodyBuilder.ToMessageBody();

                _logger.LogInformation("Sending message to {Recipient}...", "aryankhatiwoda9@gmail.com");
                var response = await client.SendAsync(message);
                _logger.LogInformation("Message sent successfully. Server response: {Response}", response);

                _logger.LogInformation("Disconnecting from server...");
                await client.DisconnectAsync(true);
                _logger.LogInformation("Disconnected successfully");

                return Ok(new
                {
                    success = true,
                    message = "Email sent successfully via direct MailKit",
                    timestamp = DateTime.Now,
                    serverResponse = response,
                    recipient = "aryankhatiwoda9@gmail.com"
                });
            }
            catch (MailKit.Security.AuthenticationException authEx)
            {
                _logger.LogError(authEx, "Authentication failed: {Message}", authEx.Message);
                return BadRequest($"Authentication failed - check your Gmail credentials and app password: {authEx.Message}");
            }
            catch (MailKit.Net.Smtp.SmtpCommandException smtpEx)
            {
                _logger.LogError(smtpEx, "SMTP command failed: {Message}, StatusCode: {StatusCode}", smtpEx.Message, smtpEx.StatusCode);
                return BadRequest($"SMTP error: {smtpEx.Message} (Status: {smtpEx.StatusCode})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending direct email: {Message}", ex.Message);
                return BadRequest($"Unexpected error: {ex.Message}");
            }
        }
    }
}