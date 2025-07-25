using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssetHub.BackgroundJobs
{
    public class AssetApprovalEmailWorker : BackgroundService
    {
        private readonly MailKitEmailSender _emailSender;

        public AssetApprovalEmailWorker(MailKitEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Example logic
                await _emailSender.SendEmailAsync(
                    "jacorow761@calorpg.com",
                    "Asset Approval Reminder",
                    "<b>Please review pending asset approvals.</b>"
                );

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken); // run every 10 min
            }
        }
    }
}