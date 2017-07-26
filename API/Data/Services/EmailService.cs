using Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class EmailService : IEmailService
    {
        public void SendNewAccountEmail(string fullname, string email, string password, Organization organization)
        {
            MailMessage message = new MailMessage(organization.EmailSettings.EmailUsername, email);
            message.Subject = $"Shiftplanning: Your new account for {organization.Name}";
            message.Body = $"Dear {fullname}!<br>An account has been created for you with the given password:<br>{password}<br><br>Use this next time you log in!";
            SendEmail(message, organization);
        }

        public void SendNewPasswordEmail(string fullname, string email, string password, Organization organization)
        {
            MailMessage message = new MailMessage(organization.EmailSettings.EmailUsername, email);
            message.Subject = $"Shiftplanning: Your updated account for {organization.Name}";
            message.Body = $"Dear {fullname}!<br>You have been assigned a new password:<br>{password}<br><br>Use this next time you log in!";
            SendEmail(message, organization);
        }

        private void SendEmail(MailMessage mail, Organization organization)
        {
            mail.BodyEncoding = Encoding.UTF8;
            mail.SubjectEncoding = Encoding.UTF8;

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body);
            htmlView.ContentType = new ContentType("text/html");
            mail.AlternateViews.Add(htmlView);

            mail.From = new MailAddress(organization.EmailSettings.EmailUsername, organization.Name);

            using (var client = new SmtpClient(organization.EmailSettings.EmailHost))
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(organization.EmailSettings.EmailUsername, organization.EmailSettings.EmailUsername);
                client.Port = organization.EmailSettings.Port;
                client.EnableSsl = true;
                client.Send(mail);
            }
        }
    }
}
