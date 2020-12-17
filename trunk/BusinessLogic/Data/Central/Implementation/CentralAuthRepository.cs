using Microsoft.Extensions.Configuration;
using BusinessModel.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BusinessLogicLayer.Data.Framework;

namespace BusinessLogicLayer.Data.Central.Implementation
{
	public class CentralAuthRepository : DataAccessComponent, ICentralAuthRepository
	{
		public CentralAuthRepository(IConfiguration configuration)
			: base(configuration, true)
		{
		}

		/// <inheritdoc />
		public AuthCredentials FetchCredentialsForUsername(string username)
		{
			using var connection = CreateConnection();
			using DbCommand command = CreateCommand(connection, "Auth.pAppUser_FetchCredentials");
			AddParameterToCommand(command, "@Username", DbType.String, username, 100);
			using var reader = command.ExecuteReader();
			if (!reader.Read())
				return null;
			return Factory.ConstructAuthCredentials(reader);
		}

		/// <inheritdoc />
		public bool CheckIfUsernameExists(string username)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public void AddFailedLoginAttempt(long userId)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public void StartPasswordReset(long userId, DateTime issued, string token)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public bool FinishPasswordReset(long userId, long passwordResetId, string newHash, string newSalt)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public long? FetchPasswordResetIdByToken(string token)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public void AddAppUser(AppUser appUser)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUser_Add");
			AddOutputParameterToCommand(command, "@AppUserId", DbType.Int64, 8);
			AddOutputParameterToCommand(command, "@ExternalId", DbType.Guid, 36);
			AddParameterToCommand(command, "@Username", DbType.String, appUser.Username, 100);
			AddParameterToCommand(command, "@PasswordHash", DbType.String, appUser.PasswordHash, 200);
			AddParameterToCommand(command, "@FailedPasswordCount", DbType.Int16, appUser.FailedPasswordCount, 2);
			AddParameterToCommand(command, "@IsAccountLocked", DbType.Boolean, appUser.IsAccountLocked, 1);
			AddParameterToCommand(command, "@EmailAddress", DbType.String, appUser.EmailAddress, 150);
			command.ExecuteNonQuery();
			appUser.AppUserId = (long)GetParameterValue(command, "@AppUserId");
			appUser.ExternalId = (Guid)GetParameterValue(command, "@ExternalId");
		}

		/// <inheritdoc />
		public void ModifyAppUser(AppUser appUser)
		{
			using var connection = CreateConnection();
			using DbCommand command = CreateCommand(connection, "Auth.pAppUser_Modify");
			AddParameterToCommand(command, "@AppUserId", DbType.Int64, appUser.AppUserId, 8);
			AddParameterToCommand(command, "@EmailAddress", DbType.String, appUser.EmailAddress, 150);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public void RemoveAppUser(long appUserId)
		{
			using var connection = CreateConnection();
			using DbCommand command = CreateCommand(connection, "Auth.AppUser_Remove");
			AddParameterToCommand(command, "@AppUserId", DbType.Int64, appUserId);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public AppUser FetchAppUserById(long appUserId)
		{
			using var connection = CreateConnection();
			using DbCommand command = CreateCommand(connection, "Auth.pAppUser_Fetch");
			AddParameterToCommand(command, "@AppUserId", DbType.Int64, appUserId);
			using IDataReader reader = command.ExecuteReader();
			if (!reader.Read())
				return null;
			return Factory.ConstructAppUser(reader);
		}

		/// <inheritdoc />
		public List<AppUser> FetchAppUserAll()
		{
			using var connection = CreateConnection();
			using DbCommand command = CreateCommand(connection, "Auth.pAppUser_Fetch");
			AddParameterToCommand(command, "@AppUserId", DbType.Int64, 0);
			using IDataReader reader = command.ExecuteReader();
			List<AppUser> entities = new List<AppUser>();
			while (reader.Read())
			{
				entities.Add(Factory.ConstructAppUser(reader));
			}
			return entities;
		}

		/// <inheritdoc />
		public bool CheckUsernameAppUser(string username)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUser_CheckUsername");
			AddParameterToCommand(command, "@Username", DbType.String, username.ToLowerInvariant(), 100);
			AddOutputParameterToCommand(command, "@IsUsed", DbType.Boolean, 1);
			command.ExecuteNonQuery();
			return GetParameterValue<bool>(command, "@IsUsed");
		}


		/// <inheritdoc />
		public void ChangePassword(long appUserId, string passwordHash)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUser_ChangePassword");
			AddParameterToCommand(command, "@AppUserId", DbType.Int64, appUserId, 8);
			AddParameterToCommand(command, "@PasswordHash", DbType.String, passwordHash, 200);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public void AddAppUserDevice(AppUserDevice appUserDevice)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUserDevice_Add");
			AddOutputParameterToCommand(command, "@AppUserDeviceId", DbType.Guid, 36);
			AddParameterToCommand(command, "@AppUserId", DbType.Int64, appUserDevice.AppUserId, 8);
			AddParameterToCommand(command, "@LastLoginDate", DbType.DateTime, appUserDevice.LastLoginDate, 8);
			AddParameterToCommand(command, "@LastIp", DbType.String, appUserDevice.LastIp, 20);
			AddParameterToCommand(command, "@DeviceToken", DbType.String, appUserDevice.DeviceToken, 200);
			AddParameterToCommand(command, "@IsBlocked", DbType.Boolean, appUserDevice.IsBlocked, 1);
			AddParameterToCommand(command, "@DevicePlatformId", DbType.Byte, appUserDevice.DevicePlatformId, 1);
			AddParameterToCommand(command, "@DeviceModel", DbType.String, appUserDevice.DeviceModel, 30);
			AddParameterToCommand(command, "@OsVersion", DbType.String, appUserDevice.OsVersion, 10);
			AddParameterToCommand(command, "@IsTrusted", DbType.Boolean, appUserDevice.IsTrusted, 1);
			command.ExecuteNonQuery();
			appUserDevice.AppUserDeviceId = GetParameterValue<Guid>(command, "@AppUserDeviceId");
		}

		/// <inheritdoc />
		public void SetIsTrustedAppUserDevice(Guid appUserDeviceId, bool isTrusted)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUserDevice_SetIsTrusted");
			AddParameterToCommand(command, "@AppUserDeviceId", DbType.Guid, appUserDeviceId, 36);
			AddParameterToCommand(command, "@IsTrusted", DbType.Boolean, isTrusted, 1);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public void SetIsBlockedAppUserDevice(Guid appUserDeviceId, bool isBlocked)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUserDevice_SetIsBlocked");
			AddParameterToCommand(command, "@AppUserDeviceId", DbType.Guid, appUserDeviceId, 36);
			AddParameterToCommand(command, "@IsBlocked", DbType.Boolean, isBlocked, 1);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public void ValidateTokenAppUserDevice(AppUserDevice appUserDevice)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUserDevice_ValidateToken");
			AddOutputParameterToCommand(command, "@AppUserDeviceId", DbType.Guid, 36);
			AddParameterToCommand(command, "@AppUserId", DbType.Int64, appUserDevice.AppUserId, 8);
			AddParameterToCommand(command, "@LastLoginDate", DbType.DateTime, appUserDevice.LastLoginDate, 8);
			AddParameterToCommand(command, "@LastIp", DbType.String, appUserDevice.LastIp, 20);
			AddParameterToCommand(command, "@DeviceToken", DbType.String, appUserDevice.DeviceToken, 200);
			AddOutputParameterToCommand(command, "@IsBlocked", DbType.Boolean, 1);
			AddOutputParameterToCommand(command, "@PushNotificationToken", DbType.String, 255);
			AddOutputParameterToCommand(command, "@IsTrusted", DbType.Boolean, 1);
			command.ExecuteNonQuery();
			appUserDevice.AppUserDeviceId = GetParameterValue<Guid>(command, "@AppUserDeviceId");
			appUserDevice.IsBlocked = GetParameterValue<bool>(command, "@IsBlocked");
			appUserDevice.PushNotificationToken = GetParameterValue<string>(command, "@PushNotificationToken");
			appUserDevice.IsTrusted = GetParameterValue<bool>(command, "@IsTrusted");
		}

		/// <inheritdoc />
		public void RemoveAppUserDevice(Guid appUserDeviceId)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.AppUserDevice_Remove");
			AddParameterToCommand(command, "@AppUserDeviceId", DbType.Guid, appUserDeviceId);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public AppUserDevice FetchAppUserDeviceById(Guid appUserDeviceId)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUserDevice_Fetch");
			AddParameterToCommand(command, "@AppUserDeviceId", DbType.Guid, appUserDeviceId);
			using IDataReader reader = command.ExecuteReader();
			if (!reader.Read())
				return null;
			return Factory.ConstructAppUserDevice(reader);
		}

		/// <inheritdoc />
		public List<AppUserDevice> FetchAppUserDeviceAll()
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUserDevice_Fetch");
			AddParameterToCommand(command, "@AppUserDeviceId", DbType.Guid, Guid.Empty);
			using IDataReader reader = command.ExecuteReader();
			List<AppUserDevice> entities = new List<AppUserDevice>();
			while (reader.Read())
			{
				entities.Add(Factory.ConstructAppUserDevice(reader));
			}
			return entities;
		}

		/// <inheritdoc />
		public List<AppUserClientAccess> FetchClientAccessForAppUser(long appUserId)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Auth.pAppUser_FetchClientAccess");
			AddParameterToCommand(command, "@AppUserId", DbType.Int64, appUserId);
			using IDataReader reader = command.ExecuteReader();
			List<AppUserClientAccess> entities = new List<AppUserClientAccess>();
			while (reader.Read())
			{
				entities.Add(Factory.ConstructAppUserClientAccess(reader));
			}
			return entities;
		}

		public static class Factory
		{
			public static AuthCredentials ConstructAuthCredentials(IDataReader reader)
			{
				AuthCredentials entity = new AuthCredentials();
				entity.AppUserId = DataReaderUtility.GetValue<long>(reader, "AppUserId");
				entity.ExternalId = DataReaderUtility.GetValue<Guid>(reader, "ExternalId");
				entity.PasswordHash = DataReaderUtility.GetValue<string>(reader, "PasswordHash");
				entity.IsAccountLocked = DataReaderUtility.GetValue<bool>(reader, "IsAccountLocked");
				return entity;
			}

			public static AppUser ConstructAppUser(IDataReader reader)
			{
				AppUser entity = new AppUser();
				entity.AppUserId = DataReaderUtility.GetValue<long>(reader, "AppUserId");
				entity.ExternalId = DataReaderUtility.GetValue<Guid>(reader, "ExternalId");
				entity.Username = DataReaderUtility.GetValue<string>(reader, "Username");
				entity.FailedPasswordCount = DataReaderUtility.GetValue<short>(reader, "FailedPasswordCount");
				entity.IsAccountLocked = DataReaderUtility.GetValue<bool>(reader, "IsAccountLocked");
				entity.EmailAddress = DataReaderUtility.GetValue<string>(reader, "EmailAddress");
				return entity;
			}

			public static AppUserDevice ConstructAppUserDevice(IDataReader reader)
			{
				AppUserDevice entity = new AppUserDevice();
				entity.AppUserDeviceId = DataReaderUtility.GetValue<Guid>(reader, "AppUserDeviceId");
				entity.AppUserId = DataReaderUtility.GetValue<long>(reader, "AppUserId");
				entity.LastLoginDate = DataReaderUtility.GetValue<DateTime>(reader, "LastLoginDate");
				entity.LastIp = DataReaderUtility.GetValue<string>(reader, "LastIp");
				entity.DeviceToken = DataReaderUtility.GetValue<string>(reader, "DeviceToken");
				entity.IsBlocked = DataReaderUtility.GetValue<bool>(reader, "IsBlocked");
				entity.DevicePlatformId = DataReaderUtility.GetValue<byte>(reader, "DevicePlatformId");
				entity.DeviceModel = DataReaderUtility.GetValue<string>(reader, "DeviceModel");
				entity.OsVersion = DataReaderUtility.GetValue<string>(reader, "OsVersion");
				entity.PushNotificationToken = DataReaderUtility.GetValue<string>(reader, "PushNotificationToken");
				entity.IsTrusted = DataReaderUtility.GetValue<bool>(reader, "IsTrusted");
				return entity;
			}

			public static AppUserClientAccess ConstructAppUserClientAccess(IDataReader reader)
			{
				AppUserClientAccess entity = new AppUserClientAccess();
				entity.ClientUserId = DataReaderUtility.GetValue<long>(reader, "ClientUserId");
				entity.ClientId = DataReaderUtility.GetValue<long>(reader, "ClientId");
				entity.AppRoleId = DataReaderUtility.GetValue<short>(reader, "AppRoleId");
				entity.ClientName = DataReaderUtility.GetValue<string>(reader, "ClientName");
				entity.InternalRoleName = DataReaderUtility.GetValue<string>(reader, "InternalRoleName");
				return entity;
			}
		}
	}
}