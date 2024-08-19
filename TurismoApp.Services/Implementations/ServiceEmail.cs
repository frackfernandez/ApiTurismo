using System.Net.Mail;
using System.Net;
using TurismoApp.Services.Interfaces;

namespace TurismoApp.Services.Implementations
{
    public class ServiceEmail : IServiceEmail
    {
        string smtpAddress = "smtp.mailgun.org";
        int portNumber = 587;
        bool enableSSL = true;

        string emailFrom = "";
        string password = "";
        string subject = "Verificación de correo";
        string body = "Haga clic en el siguiente enlace para verificarse:";
               
        public async Task SendEmailVerification(string email, string linkVerificacion)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = $"{body}\n{linkVerificacion}";
                mail.IsBodyHtml = false; // cuerpo HTML

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;

                    smtp.Send(mail);
                }
            }
        }


        
    }
}
