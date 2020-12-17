using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessModel.Auth.Model
{
	/// <summary>
	/// This is the response to a request to authenticate with the api
	/// </summary>
	public class AuthResponse
	{
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public long UserId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Required]
		public Guid ExternalUserId { get; set; } = Guid.Empty;

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[StringLength(100)]
		public string Username { get; set; } = "guest";

		/// <summary>
		/// 
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool AccountLocked { get; set; }
	}
}