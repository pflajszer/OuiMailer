using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OuiMailer
{
	public interface IEmailBodyBuilder
	{
		Dictionary<string, string> Placeholders { get; }
		string Template { get; }
		string[] HTMLLines { get; }
		string Run(RegexOptions regexOptions = 0);
		IEmailBodyBuilder SetTemplateString(string templateString);
		IEmailBodyBuilder SetTemplateFilePath(string path);
		IEmailBodyBuilder AddPlaceholder(string patternToMatch, string replaceWith);
		IEmailBodyBuilder RemovePlaceholder(string patternToMatch);
	}
}