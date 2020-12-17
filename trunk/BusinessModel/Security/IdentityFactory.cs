using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BusinessModel.Auth;
using Common.Security;

namespace BusinessModel.Security
{
	public class IdentityFactory
	{
		private const string ClaimUsername = "username";
		private const string ClaimEmailAddress = "email-address";
		private const string ClaimUserId = "user-id";
		private const string ClaimExternalId = "external-id";
		private const string ClaimRoles = "roles";
		private const string ClaimClientRoles = "client-roles";
		private const string ClaimClientIds = "client-ids";
		private const string ClaimCulture = "culture";
		private const string ClaimActiveClientId = "active-client-id";

		public static Func<AppUser, IDictionary<string, object>> AppUserToClaimsFactory { get; } = ConstructAppUserToClaims;

		private static IDictionary<string, object> ConstructAppUserToClaims(AppUser appUser)
		{
			return new Dictionary<string, object>
			{

				{ClaimUsername, appUser.Username},
				{ClaimEmailAddress, appUser.EmailAddress},
				{ClaimRoles, "ClientOwner"},
				{ClaimUserId, appUser.AppUserId},
				{ClaimExternalId, appUser.ExternalId},
				{ClaimCulture, appUser.AppLanguageId == 1?"en-US":"es"},
				//{ClaimClientRoles,string.Join(",",appUser.Access.Select(o=>$"{o.ClientId}|{o.AppRoleId}") )},
				{"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authenticated", true},
				{"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",appUser.Username},
				{ClaimActiveClientId,appUser.ActiveClientId}
			};
		}

		public static Func<IDictionary<string, string>, IIdentity> ClaimsToIdentityFactory { get; } = ConstructClaimsToIdentity;

		private static IIdentity ConstructClaimsToIdentity(IDictionary<string, string> claims)
		{
			string name = "Guest";
			string roles = "";
			string emailAddress = "";
			string culture = "en-US";
			long userId = 0;
			Guid externalUserId = Guid.Empty;
			long activeClientId = 0;
			if (claims.ContainsKey(ClaimUsername))
				name = claims[ClaimUsername];
			if (claims.ContainsKey(ClaimRoles))
				roles = claims[ClaimRoles];
			if (claims.ContainsKey(ClaimUserId))
				long.TryParse(claims[ClaimUserId], out userId);
			if (claims.ContainsKey(ClaimExternalId))
				externalUserId = Guid.Parse(claims[ClaimExternalId]);
			if (claims.ContainsKey(ClaimEmailAddress))
				emailAddress = claims[ClaimEmailAddress];
			if (claims.ContainsKey(ClaimCulture))
				culture = claims[ClaimCulture];
			if (claims.ContainsKey(ClaimActiveClientId))
				activeClientId = Convert.ToInt64(claims[ClaimActiveClientId]);
			return new CommonIdentity(name, roles, userId, externalUserId, emailAddress, culture,activeClientId);
		}
	}
}