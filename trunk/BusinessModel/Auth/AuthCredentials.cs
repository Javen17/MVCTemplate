using System;

namespace BusinessModel.Auth
{
	public class AuthCredentials
	{
		/// <summary>
		/// Gets or sets AppUserId
		/// </summary>
		public long AppUserId { get; set; }

		/// <summary>
		/// Gets or sets ExternalId
		/// </summary>
		public Guid ExternalId { get; set; }

		/// <summary>
		/// Gets or sets PasswordHash
		/// </summary>
		public string PasswordHash { get; set; }

		/// <summary>
		/// Gets or sets PasswordSalt
		/// </summary>
		public string PasswordSalt { get; set; }

		/// <summary>
		/// Gets or sets IsAccountLocked
		/// </summary>
		public bool IsAccountLocked { get; set; }
	}
}