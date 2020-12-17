using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace Common.Utilities
{
    /// <summary>
    /// Represents a collection of cryptographics utility methods. This class cannot be inherited.
    /// </summary>
    public static class CryptographicUtility
	{
		#region Public Members

		#region Methods

		#region Rfc2898DeriveKey Members

		/// <summary>
		/// Computes a 256-byte password-based key derivation, PBKDF2, by using a pseudo-random number generator based on <see cref="HMACSHA1"/> using an iteration count of 100.
		/// </summary>
		/// <param name="passphrase"></param>
		/// <param name="salt"></param>
		/// <returns></returns>
		public static byte[] Rfc2898DeriveKey(string passphrase, out byte[] salt)
		{
			salt = GenerateSalt(32);
			return Rfc2898DeriveKey(passphrase, salt, 256);
		}

		/// <summary>
		/// Computes a password-based key derivation, PBKDF2, by using a pseudo-random number generator based on <see cref="HMACSHA1"/> using an iteration count of 100.
		/// </summary>
		/// <param name="passphrase"></param>
		/// <param name="salt"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static byte[] Rfc2898DeriveKey(string passphrase, byte[] salt, int size)
		{
			return Rfc2898DeriveKey(passphrase, salt, size, 100);
		}

		/// <summary>
		/// Computes a password-based key derivation, PBKDF2, by using a pseudo-random number generator based on <see cref="HMACSHA1"/>.
		/// </summary>
		/// <param name="passphrase"></param>
		/// <param name="salt"></param>
		/// <param name="size"></param>
		/// <param name="iterations"></param>
		/// <returns></returns>
		/// <remarks>Please see the <see cref="Rfc2898DeriveBytes"/> class for additional remarks.</remarks>
		public static byte[] Rfc2898DeriveKey(string passphrase, byte[] salt, int size, int iterations)
		{
			if (iterations < 1)
			{
				throw new ArgumentOutOfRangeException("iterations", iterations, "The number of iterations must be greater than zero.");
			}

			Rfc2898DeriveBytes inputEquivalent = new Rfc2898DeriveBytes(passphrase, salt);
			inputEquivalent.IterationCount = iterations;
			return inputEquivalent.GetBytes(size);
		}

		#endregion

		#region Salt

		/// <summary>
		/// Generates a cryptographically random salt array.
		/// </summary>
		/// <param name="size">The size of the generated salt in bytes.</param>
		/// <returns></returns>
		public static byte[] GenerateSalt(int size)
		{
			if (size <= 0)
			{
				throw new ArgumentOutOfRangeException("size", size, "Size must be greater than zero.");
			}

			//
			// Create the result array and fill it with non-zero bytes.
			//
			byte[] result = new byte[size];

			// 
			// Use a cryptographic random number generator       
			//
			RNGCryptoServiceProvider.Create().GetNonZeroBytes(result);
			return result;
		}

		#endregion

		#region X.509 Signature Verification

		/// <summary>
		/// Verify the signature of an <see cref="XmlNode"/> and returns the result.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="publicKeyXml"></param>
		/// <returns></returns>
		[GeneratedCode("", "1.0.0.0")]
		public static bool VerifySignature(XmlElement element, string publicKeyXml)
		{
			SignedXml signedXml = new SignedXml();
			signedXml.LoadXml(element);

			DSA dsa = DSA.Create();
			dsa.FromXmlString(publicKeyXml);
			KeyInfo keyInfo = new KeyInfo();
			keyInfo.AddClause(new DSAKeyValue(dsa));
			signedXml.KeyInfo = keyInfo;

			return signedXml.CheckSignature();
		}

		#endregion

		#endregion

		#endregion

		/// <summary>
		/// Generates a cryptographically random human-readable passphrase.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static string GenerateRandomPassphrase(int size)
		{
			HexadecimalEncoding encoding = new HexadecimalEncoding();
			return encoding.GetString(GenerateSalt(size));
		}

		private static readonly char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

		public static string sha256(string input)
		{
			SHA256Managed crypt = new SHA256Managed();
			Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
			Byte[] hashedBytes = crypt.ComputeHash(inputBytes);

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < hashedBytes.Length; i++)
			{
				sb.Append(String.Format("{0:x2}", hashedBytes[i]));
			}

			return sb.ToString();
			/*
			byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
			foreach (byte bit in crypto)
			{
				hash.Append(bit.ToString("x2"));
			}
			return hash.ToString();*/
		}

		/// <summary>
		/// Generates a unique key.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static string GenerateUniqueKey(int size)
		{
			byte[] data = new byte[1];
			RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
			crypto.GetNonZeroBytes(data);
			data = new byte[size];
			crypto.GetNonZeroBytes(data);
			StringBuilder buffer = new StringBuilder(size);
			foreach (byte b in data)
			{
				buffer.Append(_chars[b % (_chars.Length - 1)]);
			}
			return buffer.ToString();
		}

		/// <summary>
		/// Gets the cryptographic random number.
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public static int GetRandomNumber()
		{
			return GetRandomNumber(0, int.MaxValue);
		}

		/// <summary>
		/// Gets the cryptographic random number.
		/// </summary>
		/// <param name="lBound">The l bound.</param>
		/// <param name="uBound">The u bound.</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static int GetRandomNumber(int lBound, int uBound)
		{
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

			//
			// Assumes lBound >= 0 && lBound < uBound
			//
			// returns an int >= lBound and < uBound
			//

			uint urndnum;
			byte[] rndnum = new Byte[4];

			if (lBound == uBound - 1)
			{
				//
				// Test for degenerate case where only lBound can be returned
				//
				return lBound;
			}

			uint xcludeRndBase = (uint.MaxValue - (uint.MaxValue % (uint)(uBound - lBound)));

			do
			{
				rng.GetBytes(rndnum);
				urndnum = BitConverter.ToUInt32(rndnum, 0);
			} while (urndnum >= xcludeRndBase);
			return (int)(urndnum % (uBound - lBound)) + lBound;
		}

		private static byte[] CreateKeyFromPassword(string password)
		{
			SHA256Managed hasher = new SHA256Managed();
			byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
			return hasher.ComputeHash(pwdBytes);
		}

		#region Rijndael

		/// <summary>
		/// Encrypts the specified <see cref="String"/> using the Rijndael symmetric key algorithm with the specified key and IV.
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <param name="passphrase">The passphrase.</param>
		/// <param name="salt">The salt.</param>
		/// <returns></returns>
		public static string RijndaelEncrypt(string plainText, string passphrase, string salt)
		{
			return Convert.ToBase64String(RijndaelEncrypt(UTF8Encoding.UTF8.GetBytes(plainText), ASCIIEncoding.ASCII.GetBytes(passphrase), ASCIIEncoding.ASCII.GetBytes(salt)));
		}

		/// <summary>
		/// Encrypts the specified byte array using the Rijndael symmetric key algorithm with the specified key and IV.
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <param name="passphrase">The passphrase.</param>
		/// <param name="salt">The salt.</param>
		/// <returns></returns>
		public static byte[] RijndaelEncrypt(byte[] plainText, byte[] passphrase, byte[] salt)
		{
			RijndaelManaged rijndael = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256 };

			//
			// Get an encryptor.
			//
			Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passphrase, salt, 100);
			byte[] bytes = password.GetBytes(256 / 8); // Specify the size of the key in bytes (instead of bits).

			ICryptoTransform encryptor = rijndael.CreateEncryptor(bytes, salt);

			//
			// Encrypt the data.
			//
			using (MemoryStream buffer = new MemoryStream())
			{
				CryptoStream cs = new CryptoStream(buffer, encryptor, CryptoStreamMode.Write);

				//
				// Write all data to the crypto stream and flush it.
				//
				cs.Write(plainText, 0, plainText.Length);
				cs.FlushFinalBlock();

				return buffer.ToArray();
			}
		}

		/// <summary>
		/// Decryptes the previously encrypted byte array using the Rijndael symmetric key algorithm with the specified key and IV.
		/// </summary>
		/// <param name="cipherText">The cipher text.</param>
		/// <param name="passphrase">The passphrase.</param>
		/// <param name="salt">The salt.</param>
		/// <returns></returns>
		public static string RijndaelDecrypt(string cipherText, string passphrase, string salt)
		{
			int bytesDecrypted;
			byte[] plainText = RijndaelDecrypt(Convert.FromBase64String(cipherText), ASCIIEncoding.ASCII.GetBytes(passphrase), ASCIIEncoding.ASCII.GetBytes(salt), out bytesDecrypted);
			return UTF8Encoding.UTF8.GetString(plainText, 0, bytesDecrypted);
		}

		/// <summary>
		/// Decryptes the previously encrypted byte array using the Rijndael symmetric key algorithm with the specified key and IV.
		/// </summary>
		/// <param name="cipherText">The cipher text.</param>
		/// <param name="passphrase">The passphrase.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="bytesDecrypted">The bytes decrypted.</param>
		/// <returns></returns>
		public static byte[] RijndaelDecrypt(byte[] cipherText, byte[] passphrase, byte[] salt, out int bytesDecrypted)
		{
			RijndaelManaged rijndael = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256 };
			Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passphrase, salt, 100);
			byte[] bytes = password.GetBytes(256 / 8); // Specify the size of the key in bytes (instead of bits).
			ICryptoTransform decryptor = rijndael.CreateDecryptor(bytes, salt);

			using (MemoryStream buffer = new MemoryStream(cipherText))
			{
				CryptoStream cs = new CryptoStream(buffer, decryptor, CryptoStreamMode.Read);

				byte[] plainText = new byte[cipherText.Length];

				//
				// Read the data out of the crypto stream.
				//
				bytesDecrypted = cs.Read(plainText, 0, plainText.Length);
				return plainText;
			}
		}

		#endregion

		#region Rijndael (Simple)

		/// <summary>
		/// 
		/// </summary>
		private const int BLOCK_SIZE_BITS = 128;

		/// <summary>
		/// Encrypts the specified string.
		/// </summary>
		/// <param name="plainText">The string to encrypt.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public static string RijndaelEncrypt(string plainText, string password)
		{
			return RijndaelEncrypt(plainText, CreateKeyFromPassword(password));
		}

		/// <summary>
		/// Encrypts the specified string.
		/// </summary>
		/// <param name="plainText">The string to encrypt.</param>
		/// <param name="password">The length must be a multiple of 32 and no more than 256.</param>
		/// <returns></returns>
		public static string RijndaelEncrypt(string plainText, byte[] password)
		{
			byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);

			//Genarate a memory stream on which cryptographic transformation will be performed. 
			MemoryStream memStream = new MemoryStream();

			try
			{
				SymmetricAlgorithm crypto = SymmetricAlgorithm.Create("Rijndael");
				try
				{
					//Generate a key from password and assign it to algorithm. 
					crypto.Key = password;
					crypto.BlockSize = BLOCK_SIZE_BITS; // This is 128 in this particular case 
					crypto.GenerateIV();

					crypto.Mode = CipherMode.ECB;
					crypto.Padding = PaddingMode.PKCS7;

					//Generates a stream which links data streams to cryptographic transformations. 
					CryptoStream encryptionStream = new CryptoStream(memStream,
						crypto.CreateEncryptor(crypto.Key, crypto.IV),
						CryptoStreamMode.Write);

					//Write transformation to byte array. 
					encryptionStream.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
					encryptionStream.Close();

					byte[] encryptedArray = memStream.ToArray();
					//Convert byte encrypted byte array to string. 
					string res = Convert.ToBase64String(encryptedArray);
					//return encrypted stream. 
					return res;
				}
				finally
				{
					crypto.Clear();
				}
			}
			finally
			{
				if (memStream != null)
				{
					memStream.Close();
					memStream.Dispose();
				}
			}
		}

		/// <summary>
		/// Decrypts the specified string.
		/// </summary>
		/// <param name="cypherText">The string to decrypt.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public static string RijndaelDecrypt(string cypherText, string password)
		{
			byte[] bytesToDecrypt = Convert.FromBase64String(cypherText);

			//Genarate a memory stream on which cryptographic transformation will be performed. 
			MemoryStream memStream = new MemoryStream();
			try
			{
				SymmetricAlgorithm crypto = SymmetricAlgorithm.Create("Rijndael");
				try
				{
					crypto.Key = CreateKeyFromPassword(password);
					crypto.BlockSize = BLOCK_SIZE_BITS; // This is 128 in this particular case 
														//Generates the initialization vector to be used by the algorithim.crypto.GenerateIV();     

					crypto.Mode = CipherMode.ECB;
					crypto.Padding = PaddingMode.PKCS7;

					//Generates a stream which links data streams to cryptographic transformations. 
					CryptoStream decryptionStream = new CryptoStream(memStream,
						crypto.CreateDecryptor(crypto.Key, crypto.IV),
						CryptoStreamMode.Write);

					//Write transformation to byte array.   
					decryptionStream.Write(bytesToDecrypt, 0, bytesToDecrypt.Length);
					decryptionStream.FlushFinalBlock();
					decryptionStream.Close();

					string res = Encoding.UTF8.GetString(memStream.ToArray());
					return res;
				}
				finally
				{
					crypto.Clear();
				}
			}
			finally
			{
				if (memStream != null)
				{
					memStream.Close();
					memStream.Dispose();
				}
			}
		}

		#endregion

		#region SHA1 Hash

		/// <summary>
		/// Computes the SH a1 hash.
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <returns></returns>
		public static string SHA1HashCompute(string plainText)
		{
			SHA1 sha1 = SHA1.Create();
			byte[] hashBytes = sha1.ComputeHash(Encoding.Unicode.GetBytes(plainText));
			sha1.Clear();

			// Convert result into a base64-encoded string.
			string hashValue = Convert.ToBase64String(hashBytes);
			return hashValue;
		}

		/// <summary>
		/// Verifies the SH a1 hash.
		/// </summary>
		/// <param name="plainText">The plain text.</param>
		/// <param name="hashValue">The hash value.</param>
		/// <returns></returns>
		public static bool SHA1HashVerify(string plainText, string hashValue)
		{
			return SHA1HashCompute(plainText).Equals(hashValue);
		}

		#endregion

		#region MDG Hash

		/// <summary>
		/// Computes the MD5 hash of the input bytes
		/// </summary>
		/// <param name="input">Bytes to hash</param>
		/// <returns>16 byte array</returns>
		public static byte[] MD5HashCreateRaw(byte[] input)
		{
			MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

			//ComputeHash returns the hash as an array of 16 bytes.
			byte[] data = md5Hasher.ComputeHash(input);
			return data;
		}


		/// <summary>
		/// Computes the MD5 hash of the input string and returns the base 64 string representation 
		/// (24 characters) of that hash.
		/// </summary>
		/// <param name="input">String to hash</param>
		/// <param name="salt">Optional string to suffix input with before hashing.  Null is allowed.</param>
		/// <returns>24-character, base 64-formatted string</returns>
		public static string MD5HashCreate(string input, string salt)
		{
			return MD5HashCreate(Encoding.Default.GetBytes(string.Concat(input, salt)));
		}

		/// <summary>
		/// Computes the base 64 encoded MD5 hash of the input bytes.
		/// </summary>
		/// <param name="input">bytes (including any salt) to hash</param>
		/// <returns>24-character, base 64-formatted string</returns>
		public static string MD5HashCreate(byte[] input)
		{
			byte[] data = MD5HashCreateRaw(input);
			string b64 = Convert.ToBase64String(data);
			return b64;
		}

		/// <summary>
		/// Some MD5 implementations produce a 32-character, hexadecimal-formatted hash.
		/// To interoperate with such implementations use this method.
		/// </summary>
		/// <param name="input">String to hash</param>
		/// <param name="salt">Optional string to suffix input with before hashing.  Null is allowed.</param>
		/// <returns>32-character, hexadecimal-formatted hash</returns>
		public static string MD5HashCreateAsHex(string input, string salt)
		{
			byte[] data = MD5HashCreateRaw(Encoding.Default.GetBytes(string.Concat(input, salt)));
			StringBuilder sb = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
				sb.Append(data[i].ToString("x2"));

			return sb.ToString();
		}

		/// <summary>
		/// Computes the base 64 encoded MD5 hash of the input + salt and compares it to the hash.
		/// </summary>
		public static bool MD5HashVerify(string input, string salt, string hash)
		{
			string hashOfInput = MD5HashCreate(input, salt);
			return string.Equals(hashOfInput, hash, StringComparison.Ordinal);
		}

		/// <summary>
		/// Computes the 32-character, hexadecimal-formatted MD5 hash of the input + salt and compares it to the hash.
		/// </summary>
		public static bool MD5HashVerifyAsHex(string input, string salt, string hash)
		{
			string hashOfInput = MD5HashCreateAsHex(input, salt);
			return string.Equals(hashOfInput, hash, StringComparison.Ordinal);
		}

		#endregion

		#region AesCryptoServiceProvider
		/// <summary>
		/// Encrypts the value using AesCryptoServiceProvider.
		/// </summary>
		/// <param name="plainText">The value to encrypt.</param>
		/// <param name="key">The encryption key.</param>
		/// <param name="iv">The encryptio iv.</param>
		/// <returns>The encrypted value.</returns>
		public static string AesEncrypt(string plainText, string key, string iv)
		{
			AesCryptoServiceProvider provider = CreateAesProvider(key, iv);

			MemoryStream ms = new MemoryStream();
			CryptoStream cryto = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write);
			byte[] valueToEncrypt = new ASCIIEncoding().GetBytes(plainText);
			cryto.Write(valueToEncrypt, 0, valueToEncrypt.Length);
			cryto.FlushFinalBlock();
			byte[] returnValue = ms.ToArray();
			cryto.Close();
			ms.Close();
			return Convert.ToBase64String(returnValue);
		}

		/// <summary>
		/// Decrypts the value.
		/// </summary>
		/// <param name="encryptedValue">The encrypted value.</param>
		/// <param name="key">The encryption key.</param>
		/// <param name="iv">The encryptio iv.</param>
		/// <returns>The decrypted value.</returns>
		public static string AesDecrypt(string encryptedValue, string key, string iv)
		{
			try
			{
				AesCryptoServiceProvider provider = CreateAesProvider(key, iv);

				byte[] valueToDecrypt = Convert.FromBase64String(encryptedValue);
				MemoryStream ms = new MemoryStream();
				CryptoStream crypto = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write);
				crypto.Write(valueToDecrypt, 0, valueToDecrypt.Length);
				crypto.FlushFinalBlock();
				crypto.Close();
				return new UTF8Encoding().GetString(ms.ToArray());
			}
			catch
			{
				return String.Empty;
			}
		}

		private static AesCryptoServiceProvider CreateAesProvider(string key, string iv)
		{
			AesCryptoServiceProvider provider = new AesCryptoServiceProvider();
			provider.IV = Convert.FromBase64String(iv);
			provider.Key = Convert.FromBase64String(key);
			return provider;
		}

		#endregion
	}
}