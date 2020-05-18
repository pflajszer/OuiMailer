using OuiMailer.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OuiMailer
{

	public class EmailSender : IEmailSender
	{
		public string From { get; set; }
		public string Host { get; set; }
		public Ports Port { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }

		public EmailSender(string host, string login, string password, Ports port = Ports.Modern)
		{
			Host = host;
			Port = port;
			Login = login;
			Password = password;
		}

		private SmtpClient ConfigureServices()
		{
			var client = new SmtpClient()
			{
				Host = Host,
				Port = (int)Port,
				EnableSsl = true,
				UseDefaultCredentials = false,
			};
			client.Credentials = new NetworkCredential(Login, Password);
			return client;
		}

		public async Task SendEmailAsync(MailMessage email, int dequeCount = 5)
		{
			using (SmtpClient client = ConfigureServices())
			{
				try
				{
					await client.SendMailAsync(email);
				}
				catch (Exception ex)
				{
					if (dequeCount > 0)
					{
						await SendEmailAsync(email, dequeCount - 1);
					}
					else
					{
						throw ex;
					}
				}
			}
		}

		public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true, int dequeCount = 5)
		{
			var email = new MailMessage(From, to, subject, body)
			{
				IsBodyHtml = isBodyHtml
			};
			using (SmtpClient client = ConfigureServices())
			{
				try
				{
					await client.SendMailAsync(email);
				}
				catch (Exception ex)
				{
					if (dequeCount > 0)
					{
						await SendEmailAsync(to, subject, body, isBodyHtml, dequeCount - 1);
					}
					else
					{
						throw ex;
					}
				}
			}
		}
	}
}
