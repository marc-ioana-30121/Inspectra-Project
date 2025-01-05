using PrettyNeatGenericAPI.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace PrettyNeatGenericAPI.Handlers
{
    public class SendGridHandler : IMailer
    {
        public string DefaultSender => _sender.Email;

        private EmailAddress _sender;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static ISendGridClient _client;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public SendGridHandler(IConfiguration config, ISendGridClient client)
        {
            var _config = config.GetSection("sendGrid");

            if (_config == null) throw new InvalidOperationException("Configuration could not be found");

            _sender = new EmailAddress(_config.GetValue<string>("senderEmail"));

            _client = client;
        }

        public async Task<string> Send(IMail message)
        {
            if (!string.IsNullOrEmpty(message.TemplateName))
            {
                SubstituteInTemplate(message);
            }

            var msg = new SendGrid.Helpers.Mail.SendGridMessage
            {
                From = string.IsNullOrEmpty(message.SenderOverride) ? _sender : new EmailAddress(message.SenderOverride),
                HtmlContent = message.HtmlContent,
                PlainTextContent = message.TextContent,
                Subject = message.Subject,
            };

            msg.AddTo(message.ReceiverEmail);

            if (message.Attachment != null)
            {
                var bytes = File.ReadAllBytes(message.Attachment.filePath);
                var file = Convert.ToBase64String(bytes);
                msg.AddAttachment(message.Attachment.fileName, file);
            }

            if (message.Attachments != null)
            {
                message.Attachments.ForEach(attachment => {
                    var bytes = File.ReadAllBytes(attachment.filePath);
                    var file = Convert.ToBase64String(bytes);
                    msg.AddAttachment(attachment.fileName, file);
                });
            }

            var response = await _client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
                return await response.Body.ReadAsStringAsync();

            return $"ERR:{response.StatusCode}";
        }

        public async Task<Dictionary<string, string>> Send(List<IMail> messages)
        {
            Dictionary<string, string> outcome = new Dictionary<string, string>();
            foreach (var msg in messages)
            {
                var sent = await Send(msg);
                outcome.Add(msg.ReceiverEmail, sent);
            }
            return outcome;
        }

        public (string text, string html) SubstituteInTemplate(IMail message)
        {
            //not yet implemented
            return (message.TextContent, message.HtmlContent);
        }


    }

    public class MailMessage : IMail
    {
        public string ReceiverEmail => _to;
        private string _to;

        public string SenderOverride => _from;
        private string _from;

        public string Subject => _subj;
        private string _subj;

        public string TextContent => _text;
        private string _text;

        public string HtmlContent => _html;
        private string _html;

        public string TemplateName => _template;
        private string _template;

        public SendGridAttachment Attachment => _attachment;
        private SendGridAttachment _attachment;

        public Dictionary<string, string> TemplateTokens => _templateBits;
        private Dictionary<string, string> _templateBits;

        public List<SendGridAttachment> Attachments => _attachments;
        private List<SendGridAttachment> _attachments;

        public MailMessage(
            string to,
            string subj,
            string htmlText,
            string plainText = "",
            string senderOverride = "",
            string template = "",
            SendGridAttachment attachment = null,
            Dictionary<string, string> templateData = null,
            List<SendGridAttachment> attachments = null)
        {
            _to = to;
            _subj = subj;
            _html = htmlText;
            _text = plainText;
            _from = senderOverride;
            _template = template;
            _attachment = attachment;
            _templateBits = templateData;
            _attachments = attachments;
        }
    }

    public class SendGridAttachment
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
    }
}
