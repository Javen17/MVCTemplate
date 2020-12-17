using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Common.Utilities;

namespace BusinessModel.Auth
{
	/// <summary>
	/// 
	/// </summary>
	public class AppUser
	{
		/// <summary>
		/// Gets or sets AppUserId
		/// </summary>
		[Required]
		public long AppUserId { get; set; }

		/// <summary>
		/// Gets or sets ExternalId
		/// </summary>
		public Guid ExternalId { get; set; }

		/// <summary>
		/// Gets or sets Username
		/// </summary>
		[Required]
		[StringLength(100)]
		public string Username { get; set; }

		/// <summary>
		/// The password to create the user with
		/// </summary>
		[StringLength(100)]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets FailedPasswordCount
		/// </summary>
		[Required]
		[JsonIgnore]
		public short FailedPasswordCount { get; set; }

		/// <summary>
		/// Gets or sets IsAccountLocked
		/// </summary>
		[JsonIgnore]
		public bool IsAccountLocked { get; set; }

		/// <summary>
		/// Gets or sets EmailAddress
		/// </summary>
		[Required]
		[StringLength(150)]
		[RegularExpression(RegexUtility.EmailAddress)]
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets Culture
		/// </summary>
		[JsonIgnore]
		[StringLength(10)]
		public string Culture { get; set; }

		/// <summary>
		/// Gets or sets NotificationOptions
		/// </summary>
		[JsonIgnore]
		public long NotificationOptions { get; set; }

		[JsonIgnore]
		public string PasswordHash { get; set; }

		public byte AppLanguageId { get; set; }

		[JsonIgnore]
		public AppUserClientAccess[] Access { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonIgnore]
		public long ActiveClientId { get; set; }
	}
}