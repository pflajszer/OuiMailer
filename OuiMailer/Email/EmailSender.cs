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

		public async Task SendEmailAsync(MailMessage email, SmtpClient smtpClient = null, int dequeCount = 0)
		{
			using (SmtpClient client = ConfigureServices())
			{
				try
				{
					await client.SendMailAsync(email);
				}
				catch (Exception ex)
				{
					if (dequeCount < 5)
					{
						await SendEmailAsync(email, smtpClient, dequeCount + 1);
					}
					else
					{
						throw ex;
					}
				}
			}
		}

		public async Task SendEmailAsync(string to, string subject, string msg, bool isBodyHtml = true, int dequeCount = 0)
		{
			var email = new MailMessage(From,
										to,
										subject,
										msg)
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
					if (dequeCount < 5)
					{
						await SendEmailAsync(to, subject, msg, isBodyHtml, dequeCount + 1);
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
