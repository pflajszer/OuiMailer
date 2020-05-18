using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OuiMailer
{
	public class EmailBodyBuilder : IEmailBodyBuilder
	{
		public string Template { get; internal set; }
		public Dictionary<string, string> Placeholders { get; internal set; }
		public string[] HTMLLines { get; internal set; }

		public static IEmailBodyBuilder Init()
		{
			return new EmailBodyBuilder()
			{
				Placeholders = new Dictionary<string, string>()
			};
		}

		public IEmailBodyBuilder SetTemplateString(string template)
		{
			Template = template;
			return this;
		}

		public IEmailBodyBuilder SetTemplateFilePath(string filePath)
		{
			var file = File.ReadLines(filePath);
			Template = string.Join(Environment.NewLine, file.ToArray());
			return this;
		}

		public string Run(RegexOptions regexOptions = 0)
		{
			HTMLLines = Template.Split(
				new[] { Environment.NewLine },
				StringSplitOptions.None
				);
			for (int i = 0; i < HTMLLines.Length; i++)
			{
				foreach (var ph in Placeholders)
				{
					HTMLLines[i] = new Regex(ph.Key, regexOptions).Replace(HTMLLines[i], ph.Value);
				}
			}

			string body = string.Join(String.Empty, HTMLLines.ToArray());
			return body;
		}

		public IEmailBodyBuilder AddPlaceholder(string patternToMatch, string replaceWith)
		{
			Placeholders.Add(patternToMatch, replaceWith);
			return this;
		}

		public IEmailBodyBuilder RemovePlaceholder(string patternToMatch)
		{
			Placeholders.Remove(patternToMatch);
			return this;
		}
	}
}
