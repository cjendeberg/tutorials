using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Settings;

namespace Zero99Lotto.SRC.Common.Services
{
    public class EmailSender : SendGridClient, IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly SendGridSettings _sendgridSettings;
        protected HttpClient HttpClient;

        public EmailSender(ILogger<EmailSender> logger,
            SendGridSettings sendgridSettings, SendGridClientOptions options, HttpClient httpClient) : base(options)
        {
            _logger = logger;
            _sendgridSettings = sendgridSettings;
            HttpClient = httpClient;
        }

        public async Task SendEmailAsync(string email, string subject, string message, string footer,
            List<string> ccs, string fileName = null, MemoryStream memoryStream = null)
        {
            if (string.IsNullOrEmpty(_sendgridSettings.ApiKey))
                return;

            var fromAddress = new EmailAddress(_sendgridSettings.FromEmail, _sendgridSettings.FromName);
            var toAddress = new EmailAddress(email);
            var sgMessage = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, message, message);
            if(ccs != null && ccs.Any())
                sgMessage.AddCcs(ccs.Select(x => new EmailAddress(x)).ToList());
            if (fileName != null && memoryStream != null)
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var bytes = memoryStream.ToArray();
                var base64 = Convert.ToBase64String(bytes);
                sgMessage.AddAttachment(fileName, base64);
            }
            sgMessage.SetFooterSetting(!string.IsNullOrWhiteSpace(footer), footer);
            var res = await this.SendEmailAsync(sgMessage);

            _logger.LogInformation($"[{nameof(EmailSender)}] - StatusCode:{res.StatusCode}, Email: {email}, Subject: {subject}," +
                $" Message: {message}", email, subject, message);
        }

        public override async Task<Response> MakeRequest(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await HttpClient.SendAsync(request,HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            return new Response(response.StatusCode, response.Content, response.Headers);
        }
    }
}
