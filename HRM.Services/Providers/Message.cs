﻿using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HRM.Services.Providers
{
    public class Message
    { 
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
        }
    }
}
