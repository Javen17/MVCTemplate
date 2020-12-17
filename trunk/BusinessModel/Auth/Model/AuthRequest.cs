using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessModel.Auth.Model
{
	/// <summary>
	/// The model used to authenticate a user from a device
	/// </summary>
	public class AuthRequest
	{
		/// <summary>
		/// Gets or sets Username
		/// </summary>
		[Required]
		[StringLength(100)]
		public string Username { get; set; }

		/// <summary>
		/// Gets or sets Password
		/// </summary>
		[Required]
		[StringLength(100)]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets ExternalUserId
		/// </summary>
		public Guid ExternalUserId { get; set; }

		/// <summary>
		/// Gets or sets Device
		/// </summary>
		[Required]
		public AuthDevice Device { get; set; }

		/// <summary>
		/// Gets or sets OneTimeCode
		/// </summary>
		[StringLength(20)]
		public string OneTimeCode { get; set; }
	}
}