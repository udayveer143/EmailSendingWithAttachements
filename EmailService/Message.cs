﻿using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class Message
    {
        public Message(IEnumerable<string> to,string subject, string content, IFormFileCollection attachements)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x=>new MailboxAddress(x)));
            Subject = subject;
            Content = content;
            Attachements = attachements;
        }
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection Attachements { get; set; }
    }
}
