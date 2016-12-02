using System.Text.RegularExpressions;

namespace GoodBytes.Infrastructure.Utils.Services
{
	/// <summary>
	/// Methods to remove HTML from strings.
	/// </summary>
	public static class HtmlRemovalService
	{
		/// <summary>
		/// Remove HTML from string with Regex.
		/// </summary>
		public static string StripTagsRegex(string source)
		{
			return Regex.Replace(source, "<.*?>", string.Empty);
		}

		/// <summary>
		/// Compiled regular expression for performance.
		/// </summary>
		static readonly Regex HtmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

		/// <summary>
		/// Remove HTML from string with compiled Regex.
		/// </summary>
		public static string StripTagsRegexCompiled(string source)
		{
			return HtmlRegex.Replace(source, string.Empty);
		}

		/// <summary>
		/// Remove HTML tags from string using char array.
		/// </summary>
		public static string StripTagsCharArray(string source)
		{
			var array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++)
			{
				char let = source[i];
				if (let == '<')
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (!inside)
				{
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}
	}
}