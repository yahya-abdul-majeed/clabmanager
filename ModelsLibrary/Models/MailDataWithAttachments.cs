using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Models
{
    public class MailDataWithAttachments
    {
        public List<string> To { get; } = new List<string>();
        public List<string> Bcc { get; } = new List<string>();

        public List<string> Cc { get; } = new List<string>();

        // Sender
        public string? From { get; }

        public string? DisplayName { get; }

        public string? ReplyTo { get; }

        public string? ReplyToName { get; }

        // Content
        public string Subject { get; } = string.Empty;

        public string? Body { get; }
        public IFormFileCollection? Attachments { get; set; }
    }
}
