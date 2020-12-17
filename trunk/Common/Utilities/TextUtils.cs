using System;

namespace Common.Utilities
{
	public static class TextUtils
	{
		private const char Space = ' ';
		public static string ToUpperFirst(this string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return string.Empty;
			}
			return char.ToUpper(input[0]) + input.Substring(1);
		}
		public static string ToPascalCase(this string source)
		{
			if (string.IsNullOrWhiteSpace(source))
			{
				return string.Empty;
			}
			string test = source.Replace("\\", " ").Replace("-", " ");
			string[] parts = test.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i] = parts[i].ToUpperFirst();
			}
			return string.Join("", parts);
		}
		public static string ToCamalCase(this string source)
		{
			if (string.IsNullOrWhiteSpace(source))
			{
				return string.Empty;
			}
			return char.ToLower(source[0]) + source.Substring(1);
		}
		/// <summary>
		/// Used to transform a pascal case string (default casing that c# uses) 
		/// to human readable ie with spaces on every capital letter
		/// </summary>
		/// <param name="source">pascal case string</param>
		/// <returns></returns>
		public static string PascalToHuman(this string source)
		{
			if (source == null)
				return null;
			string human = "";
			char[] characters = source.ToCharArray();
			for (int i = 0; i < characters.Length; i++)
			{
				char c = characters[i];
				if (char.IsUpper(c) && i != 0)
					human += Space;
				human += c;
			}
			return human;
		}

		/// <summary>
		/// Used to transform a pascal case string (default casing that c# uses) 
		/// to human readable ie with spaces on every capital letter
		/// </summary>
		/// <param name="source">pascal case string</param>
		/// <returns></returns>
		public static string PascalToUnderscore(this string source)
		{
			string human = "";
			char[] characters = source.ToCharArray();
			for (int i = 0; i < characters.Length; i++)
			{
				char c = characters[i];
				if (char.IsUpper(c) && i != 0)
					human += '_';
				human += c;
			}
			return human.ToLower();
		}
	}
}