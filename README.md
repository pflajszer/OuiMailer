# OuiMailer
Helpful package for generating email body, configuring SMTP Client and sending emails built in `.NET Standard 2.0`.
The purpose of the library is to make it easy to send automated e-mails with dynamically amendable bodies right from within your code.
#### Configuration Made Easy
SMTP configuration is easier than ever. Choose a Port of your choice from an `enum` value (set by default to the right one, probably, so you don't even have to touch it!),
Then choose your host from 352 different ones I've mapped for you. You do that by accessing a static properties, i.e. `Hosts.Gmail` - sorted! You only need your login and pass now, and you're ready to go.
#### Dynamic e-mail content
Need to generate some unique content for each e-mail? No problem! You can use placeholders in your e-mail template and set them to be replaced by a variable of your choice. See examples below.

## Installation

#### Package Manager:
`Install-Package OuiMailer -Version 1.0.2`
#### .NET CLI:
`dotnet add package OuiMailer --version 1.0.2`

## Usage

The example project that uses the below code can be found under `OuiMailer.Example.csproj`. Feel free to clone and play around!

### Sending E-mails:
Initialize EmailSender class with the default constructor - that's all you need!:
```csharp
var emailer = new EmailSender(Hosts.Gmail, "yourmail-or-login@gmail.com", "yourPassword");
```
You can then construct your `MailMessage` object (from standard `System.Net.Mail` library):
```csharp
var email = new MailMessage("fromEmail@gmail.com", "toEmail@yahoo.co.uk", "subject", "<p>This is e-mail body!</p>")
{
	IsBodyHtml = true
};
```
You can then simply call the `SendEmailAsync` asynchronous method, and that's it!:
```csharp
await emailer.SendEmailAsync(email);
```

... Or use a second overload that only takes 3 parameters - `string to`, `string subject`, `string body`:
```csharp
emailer.From = "yourEmail@gmail.com";
await emailer.SendEmailAsync("toEmail@yahoo.co.uk", "testing2nd overload", exampleEmailBody2);
```

### Dynamically Generating Content:

Generating a body is really easy. Initialize the builder using `EmailBodyBuilder.Init()`, then set a template - it can be a `string` (`SetTemplateString()`) or a `file` (`SetTemplateFilePath()`,
then, all you have to do is add placeholders by calling `AddPlaceholder(string pattern, string replaceWith)` method. Once you have them all, call `Run()` method. You can optionally pass a `RegexOptions` enum (see example 2)

The code below builds an email directly from the HTML string. Notice the placeholders DATETIMEYEARNOW and HOWISIT. They get replaced dynamically with other values
```csharp
IEmailBodyBuilder builder1 = EmailBodyBuilder.Init()
	.SetTemplateString("<html><head></head><body><p>This will be the whole email. Current year is: DATETIMEYEARNOW and this extension is HOWISIT</body></html>")
	.AddPlaceholder("DATETIMEYEARNOW", DateTime.Now.Year.ToString())
	.AddPlaceholder("HOWISIT", "awesome!");
var exampleEmailBody1 = builder1.Run();
```

The code below builds an email from a HTML template file.
```csharp
IEmailBodyBuilder builder2 = EmailBodyBuilder.Init()
	.SetTemplateFilePath(@"replace-with-the-path-of-your-choice\Template\email-template.html")
	.AddPlaceholder("you can replace this", "with any string")
	.AddPlaceholder("check out my app!", "https://www.paving-app.com");
// notice how we're using optional RegexOptions parameter to choose how we replace the placeholders
var exampleEmailBody2 = builder2.Run(RegexOptions.IgnoreCase);
```
Once you get the body, you can pass it to the `SendEmailAsync()` method as a `string msg` parameter, or include as a `Body` in the `MailMessage` object (remember to set `IsBodyHtml` flag to `true`).

##### Thanks for reading and feel free to reach out!
##### Pav
