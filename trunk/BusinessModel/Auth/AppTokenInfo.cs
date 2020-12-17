using System;

namespace BusinessModel.Auth
{
	/// <summary>
	/// Details of the auth operation
	/// </summary>
	public class AppTokenInfo
	{
		/// <summary>
		/// The bearer token to pass on all future request
		/// </summary>
		public string Token { get; set; }
		/// <summary>
		/// user name of the user if there is one.
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// The id of the user that was authenticated 
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// if the device trying to connect is blocked.
		/// </summary>
		public bool IsBlocked { get; set; }

		/// <summary>
		/// This is the device id that should be returned on all future login attempts 
		/// </summary>
		public Guid DeviceId { get; set; }

		/// <summary>
		/// This is the token to use for the cloud push messages
		/// </summary>
		public string FirebaseToken { get; set; }
	}
}
