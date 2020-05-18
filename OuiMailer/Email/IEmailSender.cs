using OuiMailer.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OuiMailer

{
	public interface IEmailSender
	{
		string From { get; set; }
		string Host { get; set; }
		Ports Port { get; set; }
		string Login { get; set; }
		string Password { get; set; }
		Task SendEmailAsync(MailMessage email, int dequeCount = 5);
		Task SendEmailAsync(string to, string subject, string msg, bool isBodyHtml, int dequeCount = 5);
	}
}