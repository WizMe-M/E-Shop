using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace E_Shop
{
    class Mail
    {
        SmtpClient smtp;
        MailMessage mailMessage;
        MailAddress from = new MailAddress("timkin.moxim@mail.ru", "System");
        Mail()
        {
            smtp = new SmtpClient()
            {
                Host = "smtp.mail.ru", 
                Port = 587,
                Credentials = new NetworkCredential("timkin.moxim@mail.ru", "SrrxUiu%UU61"),
                EnableSsl = true
            };
        }
        public Mail(string address, string message) : this()
        {
            mailMessage = new MailMessage(from, new MailAddress(address))
            {
                Subject = "Квитанция",
                Body = message
            };
        }
        public void SendMessage()
        {
            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception)
            {
                throw new Exception("Покупатель указал несуществующий адрес электронной почты. Невозможно отправить квитанцию на почту");
            }
        }
    }
}
