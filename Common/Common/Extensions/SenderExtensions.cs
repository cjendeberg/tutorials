using Autofac;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Zero99Lotto.SRC.Common.Services;
using Zero99Lotto.SRC.Common.Settings;

namespace Zero99Lotto.SRC.Common.Extensions
{
    public static class SenderExtensions
    {
        public static void AddEmailSender(this ContainerBuilder builder, SendGridSettings sendGridsettings)
        {
            builder.Register<IEmailSender>(context =>
            {
                var logger = context.Resolve<ILogger<EmailSender>>();
                var options = new SendGrid.SendGridClientOptions() { ApiKey = sendGridsettings.ApiKey };
                return new EmailSender(logger, sendGridsettings,
                    options,
                    new HttpClient());
            })
            .SingleInstance();
        }

        public static void AddSmsSender(this ContainerBuilder builder, AspSmsSettings aspSmsSettings)
        {
            builder.Register<ISmsSender>(context =>
            {
                var logger = context.Resolve<ILogger<SmsSender>>();
                return new SmsSender(logger, aspSmsSettings, new HttpClient());
            })
            .SingleInstance();
        }
    }
}
