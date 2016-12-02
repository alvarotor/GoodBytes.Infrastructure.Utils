using System.Collections.Generic;

namespace GoodBytes.Infrastructure.Utils.Interfaces
{
	public interface IUtilsInterface
	{
		string ToSeoUrl(string url);
		bool CheckRE(string name, string expression);
		bool CheckSpaces(string name);
		bool CheckAlphaNumber(string name);
		string Capitalize(string str);
		void ClearFolder(string folder, string except = "");
		void DeleteFileIfExists(string f);
		void MoveFile(string f, string i);
		void DeleteDirectoryIfExists(string f);
		void CreateDirectoryIfNotExists(string path);
		List<string> GetFilesFromDirectory(string path, string filter);
		bool IsDate(string var);
		bool IsNumeric(string var);
		int CountNumOfCharacters(string stringToSearch, char charToFind);
		string ReverseString(string s);
		string CheckRatingChars(string rating);
		double RoundTo05(double value);
		bool CheckIfHalfStar(double value);
		bool CheckIfFullStar(double value);
		string RemoveLastItemArray(string array, string[] separator);
	}
}