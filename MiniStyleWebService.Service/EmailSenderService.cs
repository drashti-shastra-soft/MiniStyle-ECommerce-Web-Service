using MiniStyleWebService.Infrastructure.IService;
using System;
using System.Net;
using System.Net.Mail;

namespace MiniStyleWebService.Service
{
    public class EmailSenderService : IEmailSenderService
    {
        public void SendEmail(string htmlBody, string toEmail)
        {
            try
            {
                var smtpClient = new SmtpClient("mail.mitsolutions.in")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("mail@mitsolutions.in", "mAiL#M@20!?"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("mail@mitsolutions.in", "MiniStyle Support"),
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
