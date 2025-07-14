using System.Net.Mail;
using System.Net;

namespace Shoppng_Tutorial.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("huyanhnguyen1211@gmail.com", "oaqoiuykcnfqmqys")
            };

            return client.SendMailAsync(
                new MailMessage(from: "huyanhnguyen1211@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }

    }
}
