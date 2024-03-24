using BookstoreAPI.Models;
using BookstoreAPI.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Pkcs;

namespace BookstoreAPI.Helpers
{
    public class MailHelper
    {
        private readonly MailSettings _mailSettings;
        public MailHelper(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }


        string pathHTMLPasswordPage = @"AdditionalFiles/sendPassword.html";


        public bool SendPassword(string toEmail, string password, string role)
        {

            using (MimeMessage emailMessage = new MimeMessage())
            {

                Console.WriteLine(File.Exists(pathHTMLPasswordPage));
                if (!File.Exists(pathHTMLPasswordPage)) return false;


                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(null, toEmail);
                emailMessage.To.Add(emailTo);


                emailMessage.Subject = "Your temporary password";

                string htmlTemplate = File.ReadAllText(pathHTMLPasswordPage);
                string textHTMLPasswordPage = htmlTemplate.Replace("{0}", role);
                textHTMLPasswordPage = textHTMLPasswordPage.Replace("{1}", password);

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = textHTMLPasswordPage;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }

            return true;
        }
    }
}
