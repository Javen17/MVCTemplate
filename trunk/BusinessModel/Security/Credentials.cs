using System;

namespace BusinessModel.Security
{
	/// <summary>
	/// This is used internally to get the credentials from the database to check a login.
	/// </summary>
	public class Credentials
	{
		public long AppUserId { get; set; }

		public Guid ExternalId { get; set; }

		public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }
	}
}