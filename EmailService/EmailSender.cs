using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfigurations _emailConfig;

        public EmailSender(EmailConfigurations emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private void Send(MimeMessage mailMessage)
        {
            using(var client=new SmtpClient())
            {
                try
                {
                    // Allow SSLv3.0 and all versions of TLS
                    client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOUTH2");
                    client.Authenticate(_emailConfig.UserName,_emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage=new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red'>{0}</h2>", message.Content) };
            if(message.Attachements!=null&& message.Attachements.Any())
            {
                byte[] fileBytes;
                foreach (var attachement in message.Attachements)
                {
                    using(var ms=new MemoryStream())
                    {
                        attachement.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    bodyBuilder.Attachments.Add(attachement.FileName, fileBytes,ContentType.Parse(attachement.ContentType));
                }
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private async Task SendAsync(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    // Allow SSLv3.0 and all versions of TLS
                    client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(emailMessage);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
