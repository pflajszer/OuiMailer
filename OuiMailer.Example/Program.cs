using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;;

namespace OuiMailer.Example
{
	class Program
	{
		static async Task Main(string[] args)
		{
			#region EXAMPLE1
			// initialize EmailSender class with the default constructor:
			var emailer1 = new EmailSender(Hosts.Gmail, "yourmail-or-login@gmail.com", "yourPassword");

			// this builds an email direcly from the string
			IEmailBodyBuilder builder1 = EmailBodyBuilder.Init()
			   .SetTemplateString("<html><head></head><body><p>This will be the whole email. Current year is: DATETIMEYEARNOW and this extension is HOWISIT</body></html>")
			   .AddPlaceholder("DATETIMEYEARNOW", DateTime.Now.Year.ToString())
			   .AddPlaceholder("HOWISIT", "awesome!");
			var exampleEmailBody1 = builder1.Run();

			// for this overload of email sending, you'll need to create your own instance of MailMessage.
			// Make sure to select IsBodyHtml flag to true if you're passing any html in.
			// Notice how you're passing the just build email body in.
			var email1 = new MailMessage("fromEmail@gmail.com", "toEmail@yahoo.co.uk", "subject", "<p>This is e-mail body!</p>")
			{
				IsBodyHtml = true
			};

			// this is the first overload of Emailer.SendEmailAsync(). The Smtp Client should be configured when you instantiated EmailSender class.
			// Just pass the MailMessage and you're ready to go.
			await emailer1.SendEmailAsync(email1);

			#endregion

			#region EXAMPLE2
			// initialize EmailSender class with the default constructor, but we'll also set a From property, 
			// as we're using different overload of SendEmailAsync
			var emailer2 = new EmailSender(Hosts.Gmail, "yourmail-or-login@gmail.com", "yourPassword")
			{
				From = "yourEmail@gmail.com"
			};

			// this builds an email from a template HTML file:
			IEmailBodyBuilder builder2 = EmailBodyBuilder.Init()
			   .SetTemplateFilePath(@"replace-with-the-path-of-your-choice\Template\email-template.html") // replace with the path to your template
			   .AddPlaceholder("you can replace this", "with any string")
			   .AddPlaceholder("check out my app!", "https://www.paving-app.com");
			// notice how we're using optional RegexOptions parameter to choose how we replace the placeholders
			var exampleEmailBody2 = builder2.Run(RegexOptions.IgnoreCase);

			// this just takes the string parameters of 'to', 'subject' and the generated message. You can also specify if email body contains html (true by default)
			// Similarly, the SmtpClient is already configured when  
			await emailer2.SendEmailAsync("toEmail@yahoo.co.uk", "testing2nd overload", exampleEmailBody2);

			#endregion EXAMPLE2
		}
	}
}
