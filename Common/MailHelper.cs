using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MailHelper
    {
        public void SendMail(string toEmail, string subject, string content)
        {
            var EmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var EmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var EmailPass = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            string body = content;

            MailMessage message = new MailMessage(new MailAddress(EmailAddress, EmailDisplayName), new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587;
                // Tạo xác thực bằng địa chỉ gmail và password
                client.Credentials = new NetworkCredential(EmailAddress, EmailPass);
                client.EnableSsl = true;
                client.Send(message);
            }
        }
    }
}
