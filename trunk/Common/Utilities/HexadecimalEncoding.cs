using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;

namespace Common.Utilities
{
	/// <summary>
	/// Represents a hexadecimal encoding of Unicode characters.
	/// </summary>
	[GeneratedCode("", "1.0.0.0")]
	public sealed class HexadecimalEncoding : Encoding
	{
		private static readonly int _numA = System.Convert.ToInt32('A');
		private static readonly int _num1 = System.Convert.ToInt32('0');
		private static readonly HexadecimalEncoding _encoding;

		/// <summary>
		/// Initializes the <see cref="HexadecimalEncoding"/> class.
		/// </summary>
		static HexadecimalEncoding()
		{
			_encoding = new HexadecimalEncoding();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HexadecimalEncoding"/> class.
		/// </summary>
		public HexadecimalEncoding()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public override int GetByteCount(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}

			int count = 0;

			//
			// Remove all none A-F, 0-9, characters.
			//
			for (int index = 0; index < value.Length; index++)
			{
				char c = value[index];
				if (IsHexDigit(c))
				{
					count++;
				}
			}

			//
			// If odd number of characters, discard last character
			//
			if (count % 2 != 0)
			{
				count--;
			}

			return count / 2; // 2 characters per byte
		}

		/// <summary>
		/// Creates a byte array from the hexadecimal string. Each two characters are combined
		/// to create one byte. First two hexadecimal characters become first byte in returned array.
		/// Non-hexadecimal characters are ignored. 
		/// </summary>
		/// <param name="value">The string to convert to a byte array.</param>
		/// <param name="discarded">The number of characters in the string that were ignored.</param>
		/// <returns>byte array, in the same left-to-right order as the hexString</returns>
		public byte[] GetBytes(string value, out int discarded)
		{
			if (null == value)
			{
				throw new ArgumentNullException("value");
			}

			discarded = 0;
			string result = string.Empty;

			//
			// Remove all none A-F, 0-9, characters
			//
			for (int i = 0; i < value.Length; i++)
			{
				char c = value[i];
				if (IsHexDigit(c))
				{
					result += c;
				}
				else
				{
					discarded++;
				}
			}

			//
			// If odd number of characters, discard last character
			//
			if (result.Length % 2 != 0)
			{
				discarded++;
				result = result.Substring(0, result.Length - 1);
			}

			int byteLength = result.Length / 2;
			byte[] bytes = new byte[byteLength];
			int j = 0;
			for (int index = 0; index < bytes.Length; index++)
			{
				string hex = new String(new[] { result[j], result[j + 1] });
				bytes[index] = ToByte(hex);
				j = j + 2;
			}
			return bytes;
		}

		/// <summary>
		/// Determines if given string is in proper hexadecimal string format
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsHexFormatted(string value)
		{
			foreach (char digit in value)
			{
				if (!IsHexDigit(digit))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true is c is a hexadecimal digit (A-F, a-f, 0-9)
		/// </summary>
		/// <param name="c">Character to test</param>
		/// <returns>true if hex digit, false if not</returns>
		public static bool IsHexDigit(Char c)
		{
			c = Char.ToUpper(c);
			int number = System.Convert.ToInt32(c);
			if (number >= _numA && number < (_numA + 6))
			{
				return true;
			}

			if (number >= _num1 && number < (_num1 + 10))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Decodes all the bytes in the specified byte array into a string.
		/// </summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <returns></returns>
		public override string GetString(byte[] bytes)
		{
			StringBuilder buffer = new StringBuilder(bytes.Length);
			for (int index = 0; index < bytes.Length; index++)
			{
				buffer.Append(bytes[index].ToString("X2"));
			}

			return buffer.ToString();
		}

		/// <summary>
		/// Converts 1 or 2 character string into equivalant byte value
		/// </summary>
		/// <param name="value">1 or 2 character string</param>
		/// <returns>byte</returns>
		private static byte ToByte(string value)
		{
			if (value.Length > 2 || value.Length <= 0)
			{
				throw new ArgumentException("Length must be 1 or 2.", "value");
			}

			return byte.Parse(value, NumberStyles.HexNumber);
		}

		/// <summary>
		/// Calculates the number of bytes produced by encoding a set of characters.  
		/// </summary>
		/// <param name="chars">The character array containing the set of characters to encode. </param>
		/// <param name="index">The index of the first character to encode.</param>
		/// <param name="count">The number of characters to encode</param>
		/// <returns></returns>
		public override int GetByteCount(char[] chars, int index, int count)
		{
			return GetByteCount(new string(chars, index, count));
		}

		/// <summary>
		///  Encodes all the characters in the specified <see cref="String"/> into a sequence of bytes.
		/// </summary>
		/// <param name="s">The <see cref="System.String"/> containing the characters to encode.</param>
		/// <returns></returns>
		public override byte[] GetBytes(string s)
		{
			int discarded;
			return GetBytes(s, out discarded);
		}

		/// <summary>
		///  Encodes a set of characters into a sequence of bytes
		/// </summary>
		/// <param name="chars">The character array containing the set of characters to encode.</param>
		/// <param name="charIndex">The index of the first character to encode.</param>
		/// <param name="charCount">The number of characters to encode.</param>
		/// <param name="bytes">The index at which to start writing the resulting sequence of bytes.</param>
		/// <param name="byteIndex">The byte array to contain the resulting sequence of bytes.</param>
		/// <returns></returns>
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if ((bytes == null) || (chars == null))
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", "Value must not be equal to null.");
			}

			if ((charIndex < 0) || (charCount < 0))
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", "Value must be greater than or equal to zero.");
			}

			if ((chars.Length - charIndex) < charCount)
			{
				throw new ArgumentOutOfRangeException("chars", "Index is greater than the size of the buffer.");
			}

			if ((byteIndex < 0) || (byteIndex > bytes.Length))
			{
				throw new ArgumentOutOfRangeException("byteIndex", byteIndex, "Value is out of range.");
			}

			if (chars.Length == 0)
			{
				return 0;
			}

			throw new NotImplementedException();
			//int discarded = 0;
			//byte[] temporary = GetBytes(new string(chars, charIndex, charCount), out discarded);

			//return bytes.Length;

			//int byteCount = bytes.Length - byteIndex;
			//if (bytes.Length == 0)
			//{
			//    bytes = new byte[1];
			//}
			//unsafe
			//{
			//    fixed (char* chRef = chars)
			//    {
			//        fixed (byte* numRef = bytes)
			//        {
			//            return GetBytes(chRef + charIndex, charCount, numRef + byteIndex, byteCount);
			//        }
			//    }
			//}
		}

		/// <summary>
		///  Decodes all the bytes in the specified byte array into a string.
		/// </summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <param name="index">The index of the first byte to decode.</param>
		/// <param name="count">The number of bytes to decode.</param>
		/// <returns></returns>
		public override string GetString(byte[] bytes, int index, int count)
		{
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Value must be greater than or equal to zero.");
			}

			if ((bytes.Length - index) < count)
			{
				throw new ArgumentOutOfRangeException("count", "Index plus count is greater than the size of the buffer.");
			}

			byte[] temporary = new byte[count];
			for (int i = 0; i < count; i++)
			{
				temporary[i] = bytes[i + index];
			}

			return GetString(temporary);
		}

		/// <summary>
		/// Gets an <see cref="Encoding"/> for the hexidecimal format.
		/// </summary>
		public static HexadecimalEncoding Hexidecimal
		{
			get
			{
				return _encoding;
			}
		}

		/// <summary>
		/// Calculates the number of characters produced by decoding a sequence of bytes. 
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Value must be greater than or equal to zero.");
			}

			if ((bytes.Length - index) < count)
			{
				throw new ArgumentOutOfRangeException("bytes", "Index is greater than the size of the buffer.");
			}

			if (bytes.Length == 0)
			{
				return 0;
			}

			throw new NotImplementedException();
		}

		/// <summary>
		/// Decodes a sequence of bytes into a set of characters.  
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="byteIndex"></param>
		/// <param name="byteCount"></param>
		/// <param name="chars"></param>
		/// <param name="charIndex"></param>
		/// <returns></returns>
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			if ((byteIndex < 0) || (byteCount < 0))
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "byteIndex" : "byteCount", "Value must be greater than or equal to zero.");
			}

			if ((bytes.Length - byteIndex) < byteCount)
			{
				throw new ArgumentOutOfRangeException("bytes", "Index is greater than the size of the buffer.");
			}

			if ((charIndex < 0) || (charIndex > chars.Length))
			{
				throw new ArgumentOutOfRangeException("charIndex", byteIndex, "Value is out of range.");
			}

			if (bytes.Length == 0)
			{
				return 0;
			}

			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the maximum number of bytes produced by encoding the specified number of characters.
		/// </summary>
		/// <param name="charCount"></param>
		/// <returns></returns>
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", charCount, "Value must be greater than or equal to zero.");
			}

			return charCount;
		}

		/// <summary>
		/// Calculates the maximum number of characters produced by decoding the specified number of bytes.
		/// </summary>
		/// <param name="byteCount"></param>
		/// <returns></returns>
		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}
	}
}