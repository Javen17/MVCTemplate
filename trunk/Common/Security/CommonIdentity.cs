using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Common.Security
{
	/// <summary>
	/// 
	/// </summary>
	public class CommonIdentity : IIdentity
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="roles"></param>
		/// <param name="userId"></param>
		/// <param name="externalUserId"></param>
		/// <param name="emailAddress"></param>
		/// <param name="culture"></param>
		public CommonIdentity(string username, string roles, long userId, Guid externalUserId, string emailAddress, string culture, long activeClientId)
		{
			Name = username;
			if (!string.IsNullOrWhiteSpace(roles))
				Roles = roles.Split(',');
			else
				Roles = new[] { "Guest" };
			IsAuthenticated = username != "Guest";
			UserId = userId;
			ExternalId = externalUserId;
			EmailAddress = emailAddress;
			Culture = culture;
			ActiveClientId = activeClientId;
		}

		/// <summary>
		/// 
		/// </summary>
		public long ActiveClientId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long UserId { get; }

		/// <summary>
		/// 
		/// </summary>
		public Guid ExternalId { get; set; }

		/// <inheritdoc />
		public string AuthenticationType { get; } = "claims";

		/// <inheritdoc />
		public bool IsAuthenticated { get; }

		/// <inheritdoc />
		public string Name { get; }

		/// <summary>
		/// 
		/// </summary>
		public string[] Roles { get; }

		public string EmailAddress { get; }

		public string Culture { get; }
	}
}
