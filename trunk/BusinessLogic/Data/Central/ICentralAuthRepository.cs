using System;
using System.Collections.Generic;
using BusinessModel.Auth;

namespace BusinessLogicLayer.Data.Central
{
	/// <summary>
	/// Defines how we interact with users
	/// </summary>
	public interface ICentralAuthRepository
	{
		#region Credentials/Login

		/// <summary>
		/// Used to get the credentials for logging in
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		AuthCredentials FetchCredentialsForUsername(string username);

		/// <summary>
		/// Used to check if a given username exists 
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		bool CheckIfUsernameExists(string username);

		/// <summary>
		/// Used to increment the failed login account for a given user
		/// </summary>
		/// <param name="userId"></param>
		void AddFailedLoginAttempt(long userId);

		/// <summary>
		/// Used to start the process of resetting a users password 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="issued"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		void StartPasswordReset(long userId, DateTime issued, string token);

		/// <summary>
		/// Used to finish the password reset process
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="passwordResetId"></param>
		/// <param name="newHash"></param>
		/// <param name="newSalt"></param>
		/// <returns></returns>
		bool FinishPasswordReset(long userId, long passwordResetId, string newHash, string newSalt);

		long? FetchPasswordResetIdByToken(string token);

		#endregion

		#region AppUser

		/// <summary>
		/// Used to add an instance of <see cref="AppUser"/>
		/// </summary>
		/// <param name="applicationUser">The instance of <see cref="AppUser" /> that you want to create</param>
		void AddAppUser(AppUser applicationUser);

		/// <summary>
		/// Used to modify an instance of <see cref="AppUser"/>
		/// </summary>
		/// <param name="applicationUser">The instance of <see cref="AppUser" /> that you want to modify</param>
		void ModifyAppUser(AppUser applicationUser);

		/// <summary>
		/// Used to add an instance of  <see cref="AppUser"/>
		/// </summary>
		/// <param name="applicationUserId">the primary key for the AppUser you want to remove</param>
		void RemoveAppUser(long applicationUserId);

		/// <summary>
		/// Used to find an instance of <see cref="AppUser"/> by passing in its primary key
		/// </summary>
		/// <param name="applicationUserId"></param>
		/// <returns>The instance of AppUser that matches the Key or null if not found</returns>
		AppUser FetchAppUserById(long applicationUserId);

		/// <summary> 
		/// Used to return all instances of <see cref="AppUser"/> that exists
		/// </summary>
		/// <returns>A collection of all the AppUsers in the system</returns>
		List<AppUser> FetchAppUserAll();

		/// <summary>
		/// Used to check if a username is in use
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		bool CheckUsernameAppUser(string username);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="appUserId"></param>
		/// <param name="passwordHash"></param>
		void ChangePassword(long appUserId, string passwordHash);

		#endregion end of AppUser

		#region AppUserDevice

		/// <summary>
		/// Used to add an instance of <see cref="AppUserDevice"/>
		/// </summary>
		/// <param name="appUserDevice">The instance of <see cref="AppUserDevice" /> that you want to create</param>
		void AddAppUserDevice(AppUserDevice appUserDevice);

		/*
		/// <summary>
		/// Used to modify an instance of <see cref="AppUserDevice"/>
		/// </summary>
		/// <param name="appUserDevice">The instance of <see cref="AppUserDevice" /> that you want to modify</param>
		void ModifyAppUserDevice(AppUserDevice appUserDevice);
		*/

		/// <summary>
		/// Used to validate a device is good and show be allowed to connect
		/// </summary>
		/// <param name="appUserDevice"></param>
		void ValidateTokenAppUserDevice(AppUserDevice appUserDevice);

		/// <summary>
		/// Used to mark a device as trusted or untrusted 
		/// </summary>
		/// <param name="appUserDeviceId"></param>
		/// <param name="isTrusted"></param>
		void SetIsTrustedAppUserDevice(Guid appUserDeviceId, bool isTrusted);

		/// <summary>
		/// Used to block or unblock a device 
		/// </summary>
		/// <param name="appUserDeviceId"></param>
		/// <param name="isBlocked"></param>
		void SetIsBlockedAppUserDevice(Guid appUserDeviceId, bool isBlocked);

		/// <summary>
		/// Used to add an instance of  <see cref="AppUserDevice"/>
		/// </summary>
		/// <param name="appUserDeviceId">the primary key for the AppUserDevice you want to remove</param>
		void RemoveAppUserDevice(Guid appUserDeviceId);

		/// <summary>
		/// Used to find an instance of <see cref="AppUserDevice"/> by passing in its primary key
		/// </summary>
		/// <param name="appUserDeviceId"></param>
		/// <returns>The instance of AppUserDevice that matches the Key or null if not found</returns>
		AppUserDevice FetchAppUserDeviceById(Guid appUserDeviceId);

		/// <summary> 
		/// Used to return all instances of <see cref="AppUserDevice"/> that exists
		/// </summary>
		/// <returns>A collection of all the AppUserDevices in the system</returns>
		List<AppUserDevice> FetchAppUserDeviceAll();

		#endregion end of AppUserDevice


		/// <summary>
		/// Returns a list of clients that a given user is allowed to access
		/// </summary>
		/// <param name="appUserId"></param>
		/// <returns></returns>
		List<AppUserClientAccess> FetchClientAccessForAppUser(long appUserId);



	}
}