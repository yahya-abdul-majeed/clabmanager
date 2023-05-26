using CLabManager_API.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using ModelsLibrary.Models;
using RazorEngineCore;
using System.Text;

namespace CLabManager_API.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;
        public MailService(IOptions<MailSettings> options)
        {
            _settings= options.Value;
        }

        public string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel)
        {
            string mailTemplate = LoadTemplate(emailTemplate);

            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedMailTemplate = razorEngine.Compile(mailTemplate);

            return modifiedMailTemplate.Run(emailTemplateModel);
        }

        public string LoadTemplate(string emailTemplate)
        {
            var baseDir = Environment.CurrentDirectory;
            var templateDir = Path.Combine(baseDir, "MailTemplates");
            var templatePath = Path.Combine(templateDir, $"{emailTemplate}.cshtml");
            using FileStream fs = new(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader sr = new(fs, Encoding.Default);
            var mailTemplate = sr.ReadToEnd();
            sr.Close();
            return mailTemplate;
        }

        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.DisplayName,mailData.From ?? _settings.From));
                message.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                foreach(var mailAddress in mailData.To)
                {
                    message.To.Add(MailboxAddress.Parse(mailAddress));
                }

                if(!string.IsNullOrEmpty(mailData.ReplyTo))
                {
                    message.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName,mailData.ReplyTo));
                }

                if(mailData.Bcc!= null)
                {
                    foreach(string mailAddress in mailData.Bcc.Where(x=>!string.IsNullOrEmpty(x)))
                    {
                        message.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                    }
                }


                if(mailData.Cc!= null)
                {
                    foreach(string mailAddress in mailData.Cc.Where(x=>!string.IsNullOrEmpty(x)))
                    {
                        message.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                    }
                }

                var body = new BodyBuilder();
                message.Subject = mailData.Subject;
                //body.HtmlBody = mailData.Body;
                message.Body = new TextPart(TextFormat.Plain) { Text = mailData.Body};

                using (var smtp = new SmtpClient())
                {
                    if (_settings.UseSSL)
                    {
                        await smtp.ConnectAsync(_settings.Host,_settings.Port,SecureSocketOptions.SslOnConnect,ct);
                    }
                    if (_settings.UseStartTls)
                    {
                        await smtp.ConnectAsync(_settings.Host,_settings.Port,SecureSocketOptions.StartTls,ct);
                    }
                    await smtp.AuthenticateAsync(_settings.UserName,_settings.Password,ct);
                    await smtp.SendAsync(message, ct);
                    await smtp.DisconnectAsync(true,ct);

                    return true;
                }



            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendWithAttachmentAsync(MailDataWithAttachments mailData, CancellationToken ct)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.DisplayName,mailData.From ?? _settings.From));
                message.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                foreach(var mailAddress in mailData.To)
                {
                    message.To.Add(MailboxAddress.Parse(mailAddress));
                }

                if(!string.IsNullOrEmpty(mailData.ReplyTo))
                {
                    message.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName,mailData.ReplyTo));
                }

                if(mailData.Bcc!= null)
                {
                    foreach(string mailAddress in mailData.Bcc.Where(x=>!string.IsNullOrEmpty(x)))
                    {
                        message.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                    }
                }


                if(mailData.Cc!= null)
                {
                    foreach(string mailAddress in mailData.Cc.Where(x=>!string.IsNullOrEmpty(x)))
                    {
                        message.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                    }
                }

                var body = new BodyBuilder();
                message.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;
                
                if(mailData.Attachments != null)
                {
                    byte[] attachmentFileByteArray;
                    
                    foreach(IFormFile attachment in mailData.Attachments)
                    {
                        if(attachment.Length> 0)
                        {
                            using (MemoryStream ms = new())
                            {
                                attachment.CopyTo(ms);
                                attachmentFileByteArray = ms.ToArray();
                                
                            }
                            body.Attachments.Add(attachment.FileName, attachmentFileByteArray,
                                ContentType.Parse(attachment.ContentType));
                        }
                    }
                }

                message.Body = body.ToMessageBody();
                using (var smtp = new SmtpClient())
                {
                    if (_settings.UseSSL)
                    {
                        await smtp.ConnectAsync(_settings.Host,_settings.Port,SecureSocketOptions.SslOnConnect,ct);
                    }
                    if (_settings.UseStartTls)
                    {
                        await smtp.ConnectAsync(_settings.Host,_settings.Port,SecureSocketOptions.StartTls,ct);
                    }
                    await smtp.AuthenticateAsync(_settings.UserName,_settings.Password,ct);
                    await smtp.SendAsync(message, ct);
                    await smtp.DisconnectAsync(true,ct);

                    return true;
                }



            }catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}
