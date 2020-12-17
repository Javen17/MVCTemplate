using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using BusinessLogicLayer.Business.Framework;
using BusinessLogicLayer.Data.Central;
using BusinessLogicLayer.Security;
using BusinessModel.Auth;
using BusinessModel.Auth.Model;
using Common;

namespace BusinessLogicLayer.Business.Implementation
{
	/// <inheritdoc cref="ICentralAuthManager"/> />
	public class CentralAuthManager : BusinessLogicComponent, ICentralAuthManager
	{


		private readonly ICentralAuthRepository _authRepository;
		private readonly IConfiguration _configuration;
		private readonly string _key;
		public CentralAuthManager(ICentralAuthRepository authRepository, IConfiguration configuration, ICommonPrincipleAccessor commonPrincipleAccessor)
		: base(commonPrincipleAccessor)
		{
			_authRepository = authRepository;
			_configuration = configuration;
			if (_configuration == null)
				//https://randomkeygen.com/
				_key = "https://randomkeygen.com/";
			else
				_key = configuration["Auth:SecretKey"];
		}

		/// <inheritdoc />
		public AppUser LogOn(AuthRequest authRequest)
		{
			authRequest.Username = authRequest.Username.ToLowerInvariant();
			var appUserCredentials = _authRepository.FetchCredentialsForUsername(authRequest.Username);
			if (appUserCredentials == null)
				return null;

			if (PasswordStorage.VerifyPassword(authRequest.Username + authRequest.Password + _key, appUserCredentials.PasswordHash))
			{
				// user credentials are valid
				var user = _authRepository.FetchAppUserById(appUserCredentials.AppUserId);
				user.Access = FetchClientAccessForAppUser(user.AppUserId).ToArray();
				return user;
			}
			return null;
		}

		/// <inheritdoc />
		public void CreateAppUser(AppUser appUser)
		{
			appUser.EmailAddress = appUser.EmailAddress.ToLowerInvariant();
			appUser.Username = appUser.Username.ToLowerInvariant();
			appUser.PasswordHash = PasswordStorage.CreateHash(appUser.Username + appUser.Password + _key);
			appUser.NotificationOptions = int.MaxValue;
			appUser.FailedPasswordCount = 0;
			appUser.IsAccountLocked = false;
			_authRepository.AddAppUser(appUser);
			appUser.PasswordHash = null;
			appUser.Password = null;
		}

		/// <inheritdoc />
		public void ChangePassword(long appUserId, string username, string newPassword)
		{
			_authRepository.ChangePassword(appUserId, PasswordStorage.CreateHash(username + newPassword + _key));
		}

		/// <inheritdoc />
		public bool CheckUsernameAppUser(string username)
		{
			return _authRepository.CheckUsernameAppUser(username);
		}

		/// <inheritdoc />
		public List<AppUserClientAccess> FetchClientAccessForAppUser(long appUserId)
		{
			return _authRepository.FetchClientAccessForAppUser(appUserId);
		}

		/// <inheritdoc />
		public List<AppUserClientAccess> FetchClientAccessForMe()
		{
			return _authRepository.FetchClientAccessForAppUser(CurrentPrincipal.CommonIdentity.UserId);
		}
	}
}