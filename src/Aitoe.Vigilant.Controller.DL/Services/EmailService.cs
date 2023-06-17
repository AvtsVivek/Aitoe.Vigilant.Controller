using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ExceptionDefs;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.BL.Services;
using Aitoe.Vigilant.Controller.DL.Entities;
using Aitoe.Vigilant.Controller.DL.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.DL.Services
{
    public class EmailService : IEmailService
    {
        public bool? IsSMTPHostSet()
        {
            var host = GetSMTPHost();
            if (
                !string.IsNullOrEmpty(host.SendersEmailAddress) &&
                !string.IsNullOrEmpty(host.EmailSendersPassword) &&
                !string.IsNullOrEmpty(host.SMTPServer) &&
                host.SMTPPort != 0
                )
            {
                return true;
            }
            else
                return false;
        }
               

        public EmailService()
        {
             
        }

        private bool bMailSent = false;
        public async Task<bool> TrySendEmailAsync(IEmail email)
        {
            ValidateEmail(email);
            bMailSent = false;
            ISMTPHost host = null;
            if (IsSMTPHostSet().HasValue && !IsSMTPHostSet().Value)
                throw new AitoeMessageServiceException(AitoeErrorCodes.SMTPHostNotSet);
            else
            {
                host = GetSMTPHost();
                ValidateSMTPHost(host);
                var fromAddress = new MailAddress(host.SendersEmailAddress);
                var message = new MailMessage();
                message.From = fromAddress;
                foreach (var toAddress in email.ToAddressList)
                {
                    var toMailAddress = new MailAddress(toAddress);
                    message.To.Add(toMailAddress);
                }

                message.Subject = email.ToTitle;
                message.Body = email.ToBody;

                // Add attachments

                foreach (var attachment in email.Attachments)
                {
                    ContentType contentType = new ContentType();
                    contentType.MediaType = MediaTypeNames.Application.Octet;
                    contentType.Name = Path.GetFileName(attachment);
                    Attachment a;
                    a = new Attachment(attachment, contentType);
                    message.Attachments.Add(a);
                }

                string fromPassword = host.EmailSendersPassword;
                var smtpClient = new SmtpClient
                {
                    Host = host.SMTPServer,
                    Port = host.SMTPPort.Value,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                smtpClient.SendCompleted += SmtpClient_SendCompleted;

                using (message)
                using (smtpClient)
                {
                    await smtpClient.SendMailAsync(message).ConfigureAwait(false);
                    return true;
                }
            }
        }

        public bool TrySendEmail(IEmail email)
        {
            ValidateEmail(email);
            bMailSent = false;
            ISMTPHost host = null;
            if (IsSMTPHostSet().HasValue && !IsSMTPHostSet().Value)
                throw new AitoeMessageServiceException(AitoeErrorCodes.SMTPHostNotSet);
            else
            {
                host = GetSMTPHost();
                ValidateSMTPHost(host);
                var fromAddress = new MailAddress(host.SendersEmailAddress);
                var message = new MailMessage();
                message.From = fromAddress;
                foreach (var toAddress in email.ToAddressList)
                {
                    var toMailAddress = new MailAddress(toAddress);
                    message.To.Add(toMailAddress);
                }

                message.Subject = email.ToTitle;
                message.Body = email.ToBody;

                // Add attachments

                foreach (var attachment in email.Attachments)
                {
                    ContentType contentType = new ContentType();
                    contentType.MediaType = MediaTypeNames.Application.Octet;
                    contentType.Name = Path.GetFileName(attachment);
                    Attachment a;
                    a = new Attachment(attachment, contentType);
                    message.Attachments.Add(a);
                }

                string fromPassword = host.EmailSendersPassword;
                var smtpClient = new SmtpClient
                {
                    Host = host.SMTPServer,
                    Port = host.SMTPPort.Value,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                smtpClient.SendCompleted += SmtpClient_SendCompleted;

                using (message)
                using (smtpClient)
                {
                    //await smtpClient.SendMailAsync(message).ConfigureAwait(false);
                    smtpClient.Send(message);
                    return true;
                }
            }
        }

        private void ValidateEmail(IEmail email)
        {
            if (email.ToAddressList.Count == 0)
                throw new AitoeMessageServiceException(AitoeErrorCodes.EmailAddressListIsInvalid);
            // And so on.
        }

        private void ValidateSMTPHost(ISMTPHost host)
        {
            if (string.IsNullOrEmpty(host.SendersEmailAddress))
                throw new AitoeMessageServiceException(AitoeErrorCodes.SendersEmailAddressIsInvalid);
            if (string.IsNullOrEmpty(host.SMTPServer))
                throw new AitoeMessageServiceException(AitoeErrorCodes.SMTPServerIsInvalid);
            // And so on.
        }

        private void SmtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            
            var token = (TaskCompletionSource<object>)e.UserState;

            if (e.Cancelled)
            {
                //Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                //Console.WriteLine("Message sent.");
            }
            bMailSent = true;
        }

        public bool? SetSMTPHost(ISMTPHost smtpHost)
        {
            var encryptionService = new EncryptionService();
            var sEncryptedPassword = string.Empty;
            if (!string.IsNullOrEmpty(smtpHost.EmailSendersPassword))
                sEncryptedPassword = encryptionService.EncryptText(smtpHost.EmailSendersPassword);
            
            Settings.Default.SendersEmailAddress = smtpHost.SendersEmailAddress;
            Settings.Default.EmailSendersPassword = sEncryptedPassword;
            Settings.Default.SMTPServer = smtpHost.SMTPServer;
            if (smtpHost.SMTPPort.HasValue && smtpHost.SMTPPort.Value != 0)
                Settings.Default.SMTPPort = smtpHost.SMTPPort.Value;

            Settings.Default.Save();
            return true;
        }

        public ISMTPHost GetSMTPHost()
        {
            var encryptionService = new EncryptionService();
            var host = new SMTPHost();
            var decryptedPass = encryptionService.DecryptText(Settings.Default.EmailSendersPassword);
            host.SendersEmailAddress = Settings.Default.SendersEmailAddress;
            host.EmailSendersPassword = decryptedPass;
            host.SMTPServer = Settings.Default.SMTPServer;
            host.SMTPPort = Settings.Default.SMTPPort;
            return host;
        }

        public bool? ResetSMTPHost()
        {
            Settings.Default.SendersEmailAddress = string.Empty;
            Settings.Default.EmailSendersPassword = string.Empty;
            Settings.Default.SMTPServer = string.Empty;
            Settings.Default.SMTPPort = 0;
            Settings.Default.Save();
            return true;
        }
        public Exception GetError()
        {
            return null;
        }
    }
}
