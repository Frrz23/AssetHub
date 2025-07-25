using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace AssetHub.Notifications
{
    public class AssetNotificationEmailJob : AsyncBackgroundJob<AssetNotificationEmailArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;

        public AssetNotificationEmailJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }


    public override async Task ExecuteAsync(AssetNotificationEmailArgs args)
    {
        await _emailSender.SendAsync(args.Email, args.Subject, args.Body);
    }


    }
}
