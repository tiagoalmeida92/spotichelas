using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace UI.Utils
{
    public static class EmailUtil
    {
        private const string HostEmail = "noreply.spotichelas@gmail.com";
        private const string Password = "isel2013";
        private const string SmtpGmail = "smtp.gmail.com";

        public static void Send(string emailAdress, string userName, string link)
        {
            using (var smtp = new SmtpClient(SmtpGmail))
            {
                using (var email = new MailMessage(HostEmail, emailAdress))
                {
                    email.Subject = "Verification Email";
                    email.Body = "Welcome to Spotichelas," + userName +
                                 "\n Activation link:" + link;

                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(HostEmail, Password);

                    smtp.Send(email);
                }
            }
        }
    }
}