using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace WPFBiblioteca.Helpers
{
    public class MailService
    {
        private readonly SmtpClient _oSmtpClient;
        public MailService()
        {
            _oSmtpClient = new SmtpClient("smtp.office365.com");
            _oSmtpClient.EnableSsl = true;
            _oSmtpClient.UseDefaultCredentials = false;
            _oSmtpClient.Host = "smtp.office365.com";
            _oSmtpClient.Port = 587;
            
        }

        public void Send(string to, string from, string pass, string body, string subject)
        {
            var oMailMessage = new MailMessage(from, to, subject, body);
            oMailMessage.IsBodyHtml = true;
            _oSmtpClient.Credentials = new NetworkCredential(from, pass);
           _oSmtpClient.Send(oMailMessage);


        }

    }
}