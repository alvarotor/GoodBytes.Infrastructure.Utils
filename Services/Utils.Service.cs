using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoodBytes.Infrastructure.Utils.Interfaces;
using Microsoft.VisualBasic;

namespace GoodBytes.Infrastructure.Utils.Services
{
	public class UtilsService : IUtilsInterface
	{
		public string ToSeoUrl(string url)
		{
#warning Change the code of this method to remove smaller words of 3 characters and ask if its too short string then leave them

			// make the url lowercase
			string encodedUrl = (url ?? "").ToLower();
			// replace & with and
			encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");
			// remove characters
			encodedUrl = encodedUrl.Replace("'", "");
			// remove ñ
			encodedUrl = encodedUrl.Replace("ñ", "n");
			encodedUrl = encodedUrl.Replace("Ñ", "N");
			// remove accents
			encodedUrl = RemoveAccentsWithRegEx(encodedUrl);
			// remove invalid characters
			encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");
			// remove duplicates
			encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");
			// trim leading & trailing characters
			encodedUrl = encodedUrl.Trim('-');
			return encodedUrl;
		}

		private static string RemoveAccentsWithRegEx(string inputString)
		{
			var replaceAAccents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
			var replaceEAccents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
			var replaceIAccents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
			var replaceOAccents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
			var replaceUAccents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
			inputString = replaceAAccents.Replace(inputString, "a");
			inputString = replaceEAccents.Replace(inputString, "e");
			inputString = replaceIAccents.Replace(inputString, "i");
			inputString = replaceOAccents.Replace(inputString, "o");
			inputString = replaceUAccents.Replace(inputString, "u");
			return inputString;
		}

		public bool CheckRE(string name, string expression)
		{
			Match match = Regex.Match(name, expression, RegexOptions.IgnoreCase);
			return match.Success;
		}

		public bool CheckSpaces(string name)
		{
			return CheckRE(name, @"^[\S]*$");
		}

		public bool CheckAlphaNumber(string name)
		{
			return CheckRE(name, @"^[a-zA-Z0-9_-]*$");
		}

		public string Capitalize(string str)
		{
			if (str.Length < 2) return str.ToUpper();
			return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
		}

		public void ClearFolder(string folder, string except = "")
		{
			var dir = new DirectoryInfo(folder);
			foreach (FileInfo fi in dir.GetFiles())
				if (except == string.Empty || fi.Name != except || !fi.Name.StartsWith(except))
					fi.Delete();
			foreach (DirectoryInfo di in dir.GetDirectories())
			{
				ClearFolder(di.FullName, except);
				di.Delete();
			}
		}

		public void CreateDirectoryIfNotExists(string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}

		public List<string> GetFilesFromDirectory(string path, string filter)
		{
			//filter: usually "*.*"
			return Directory.GetFiles(path, filter, SearchOption.TopDirectoryOnly).ToList();
		}

		public void DeleteFileIfExists(string f)
		{
			if (File.Exists(f)) File.Delete(f);
		}

		public void MoveFile(string f, string i)
		{
			if (File.Exists(f))
				File.Move(f, i);
		}

		public void DeleteDirectoryIfExists(string f)
		{
			if (Directory.Exists(f))
				Directory.Delete(f);
		}

		public bool IsDate(string var)
		{
			try
			{
				DateTime dt = DateTime.Parse(var);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool IsNumeric(string var)
		{
			try
			{
				double number;
				return Double.TryParse(var, out number);
			}
			catch
			{
				return false;
			}
		}

		public int CountNumOfCharacters(string stringToSearch, char charToFind)
		{
			int count = 0;
			char[] chars = stringToSearch.ToCharArray();
			foreach (char c in chars)
				if (c == charToFind) count++;
			return count;
		}

		public string ReverseString(string s)
		{
			char[] arr = s.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
		}

		public string CheckRatingChars(string rating)
		{
			if (string.IsNullOrEmpty(rating))
				return "0.0";
			rating = rating.Trim();
			if (rating.Length == 0)
				rating += "0.0";
			if (rating.Length == 1)
				rating += ".0";
			if (rating.Length == 2)
				rating += "0";
			return rating;
		}

		public double RoundTo05(double value)
		{
			return (Math.Ceiling(2 * value)) / 2;
		}

		public bool CheckIfHalfStar(double value)
		{
			value *= 10;
			return (value % 10) == 5;
		}

		public bool CheckIfFullStar(double value)
		{
			value *= 10;
			return ((value % 10) == 0);
		}

		public string RemoveLastItemArray(string array, string[] separator)
		{
			if (string.IsNullOrEmpty(array)) return string.Empty;
			var modelItems = array.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
			return string.Join<string>(separator.ToString(), modelItems);
		}
	}
}