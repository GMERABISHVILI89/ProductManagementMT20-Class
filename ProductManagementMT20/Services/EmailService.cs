using ProductManagementMT20.Interfaces;
using System.Net;
using System.Net.Mail;

namespace ProductManagementMT20.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(string to, string subject, string htmlMessage)
        {
			try
			{
				SmtpClient smtpClient = new SmtpClient();
				string from = "";
				try
				{
					smtpClient.UseDefaultCredentials = false;
					smtpClient.Host = "smtp.office365.com";
					smtpClient.Port = 587;
					smtpClient.EnableSsl = true;
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtpClient.Credentials = new NetworkCredential("maili", "pass");
				}
				catch (Exception)
				{
					return false;
				}

				try
				{
					MailMessage mail = new MailMessage(from, to, subject, htmlMessage);
					smtpClient.Send(mail);
					return true;
				}
				catch (Exception)
				{

					throw;
				}
			}
			catch (Exception)
			{
				return false;
			}
        }
    }
}
