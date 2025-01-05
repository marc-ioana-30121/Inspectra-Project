using PrettyNeatGenericAPI.Handlers;
using System.ComponentModel.DataAnnotations;

namespace PrettyNeatGenericAPI.Interfaces
{
    public interface IMailer
    {
        [EmailAddress]
        public string DefaultSender { get; }

        public Task<string> Send(IMail message);

        public Task<Dictionary<string, string>> Send(List<IMail> messages);

        internal (string text, string html) SubstituteInTemplate(IMail message);
    }

    public interface IMail
    {
        [Required]
        [EmailAddress]
        public string ReceiverEmail { get; }

        [EmailAddress]
        public string SenderOverride { get; }

        [MaxLength(200)]
        public string Subject { get; }

        public string TextContent { get; }

        public string HtmlContent { get; }

        public string TemplateName { get; }
        public Dictionary<string, string> TemplateTokens { get; }

        public SendGridAttachment Attachment { get; }

        public List<SendGridAttachment> Attachments { get; }
    }
}
