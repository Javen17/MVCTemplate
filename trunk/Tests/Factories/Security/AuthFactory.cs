using System;
using BusinessModel.Auth;
using Common.Utilities;

namespace Tests.Factories.Security
{
	public static class AuthFactory
	{
		private static int _userIndex = 0;
		public static AppUser CreateAppUser()
		{
			return new AppUser
			{
				EmailAddress = $"test{_userIndex++}@test.com",
				Username = $"{Guid.NewGuid()}_{_userIndex++}",
				Password = Guid.NewGuid().ToString(),
				NotificationOptions = int.MaxValue,
				Culture = "es",
				AppLanguageId = 1,
				PasswordHash = Guid.NewGuid().ToString()
			};
		}

		public static AppUserDevice CreateAppUserDevice()
		{
			return new AppUserDevice
			{
				DeviceModel = "OnePlus 7T",
				DevicePlatformId = 1,
				DeviceToken = Guid.NewGuid().ToString(),
				OsVersion = "11",
				PushNotificationToken = CryptographicUtility.GenerateRandomPassphrase(255),
				LastLoginDate = DateTime.Now.AddHours(-1),
				LastIp = "10.0.52.30",
				IsBlocked = false
			};
		}
	}
}
