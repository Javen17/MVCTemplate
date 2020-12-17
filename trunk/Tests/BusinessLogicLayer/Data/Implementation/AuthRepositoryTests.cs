using System;
using NUnit.Framework;
using BusinessLogicLayer.Data;
using BusinessLogicLayer.Data.Central;
using BusinessLogicLayer.Data.Central.Implementation;
using BusinessModel.Auth;
using Tests.Factories.Security;

namespace Tests.BusinessLogicLayer.Data.Implementation
{
	[TestFixture]
	public class AuthRepositoryTests : TestBase
	{
		#region AppUser Tests

		private AppUser ConstructAppUser(ICentralAuthRepository repo)
		{
			var entity = AuthFactory.CreateAppUser();
			// todo: build out your entity dependencies here
			return entity;
		}

		[Test]
		[Category(Integration)]
		public void CreateAppUserTest()
		{

			InvokeInTransactionScope(
			s =>
			{
				var repo = GetRepository();
				var entity = ConstructAppUser(repo);
				Assert.That(entity.AppUserId, Is.EqualTo(0));
				Assert.That(entity.ExternalId, Is.EqualTo(Guid.Empty));
				repo.AddAppUser(entity);
				Assert.That(entity.AppUserId, Is.Not.EqualTo(0));
				Assert.That(entity.ExternalId, Is.Not.EqualTo(Guid.Empty));
				Console.WriteLine($"user: {entity.Username}   pass: {entity.Password}");
				s.Complete();
			});
		}

		private ICentralAuthRepository GetRepository()
		{
			return new CentralAuthRepository(Configuration);
		}

		[Test]
		[Category(Unit)]
		public void ModifyAppUserTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructAppUser(repo);
					repo.AddAppUser(entity);
					entity.EmailAddress = "changed@test.com";
					repo.ModifyAppUser(entity);
					var readAppUser = repo.FetchAppUserById(entity.AppUserId);
					//Assert.That(entity.GetPropertyCount(), Is.EqualTo(11),
					//"please update this test as the number of properties on AppUser as changed");
					Assert.That(readAppUser.AppUserId, Is.EqualTo(entity.AppUserId));
					Assert.That(readAppUser.EmailAddress, Is.EqualTo(entity.EmailAddress));

				});
		}

		[Test]
		[Category(Integration)]
		public void FetchAppUserAllTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructAppUser(repo);
					repo.AddAppUser(entity);
					var items = repo.FetchAppUserAll();

					Assert.That(items, Is.Not.Null);
					Assert.That(items.Count, Is.GreaterThanOrEqualTo(1));
				});
		}



		[Test]
		[Category(Integration)]
		public void InvalidIdReturnsNullAppUserTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var item = repo.FetchAppUserById(int.MaxValue);
					Assert.That(item, Is.Null);
				});
		}


		#endregion

		#region AppUserDevice Tests

		private AppUserDevice ConstructAppUserDevice(ICentralAuthRepository repo)
		{
			var entity = AuthFactory.CreateAppUserDevice();
			var appUser = AuthFactory.CreateAppUser();
			repo.AddAppUser(appUser);
			entity.AppUserId = appUser.AppUserId;
			// todo: build out your entity dependencies here
			return entity;
		}

		[Test]
		[Category(Integration)]
		public void CreateAppUserDeviceTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructAppUserDevice(repo);
					Assert.That(entity.AppUserDeviceId, Is.EqualTo(Guid.Empty));
					repo.AddAppUserDevice(entity);
					Assert.That(entity.AppUserDeviceId, Is.Not.EqualTo(Guid.Empty));
				});
		}

		[Test]
		[Category(Integration)]
		public void ModifyAppUserDeviceTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructAppUserDevice(repo);
					Assert.That(entity.IsBlocked, Is.False);
					Assert.That(entity.IsTrusted, Is.False);
					repo.AddAppUserDevice(entity);
					Guid appUserDeviceId = entity.AppUserDeviceId;
					string token = entity.DeviceToken;
					long userId = entity.AppUserId;

					Assert.That(entity.IsBlocked, Is.False);
					Assert.That(entity.IsTrusted, Is.False);
					entity = repo.FetchAppUserDeviceById(appUserDeviceId);
					Assert.That(entity.IsBlocked, Is.False);
					Assert.That(entity.IsTrusted, Is.False);
					// by default we set last login to be -1 hour
					Assert.That(entity.LastLoginDate, Is.LessThan(DateTime.Now));
					repo.SetIsTrustedAppUserDevice(appUserDeviceId, true);
					// check verify device logic
					AppUserDevice device = new AppUserDevice
					{
						AppUserId = userId,
						DeviceToken = token,
						LastIp = "100.0.0.1",
						LastLoginDate = DateTime.Now.AddHours(10)
					};
					Assert.That(entity.PushNotificationToken, Is.Null);
					repo.ValidateTokenAppUserDevice(device);
					Assert.That(entity.PushNotificationToken, Is.EqualTo(entity.PushNotificationToken));
					Assert.That(device.AppUserDeviceId, Is.EqualTo(appUserDeviceId));
					Assert.That(device.IsTrusted, Is.True);
					entity = repo.FetchAppUserDeviceById(appUserDeviceId);
					Assert.That(entity.LastIp, Is.EqualTo("100.0.0.1"));
					// we set last login to be +10 hours in validate device token
					Assert.That(entity.LastLoginDate, Is.GreaterThan(DateTime.Now));

					// verify that a blocked device doesn't update any settings
					repo.SetIsBlockedAppUserDevice(appUserDeviceId, true);

					device = new AppUserDevice
					{
						AppUserId = userId,
						DeviceToken = token,
						LastIp = "101.0.0.1",
						LastLoginDate = DateTime.Now.AddHours(-10)
					};
					repo.ValidateTokenAppUserDevice(device);
					Assert.That(device.AppUserDeviceId, Is.EqualTo(appUserDeviceId));
					// verify we didn't change last login or ip
					entity = repo.FetchAppUserDeviceById(appUserDeviceId);
					Assert.That(entity.LastIp, Is.EqualTo("100.0.0.1"));
					Assert.That(entity.LastLoginDate, Is.GreaterThan(DateTime.Now));
				});
		}

		[Test]
		[Category(Integration)]
		public void FetchAppUserDeviceAllTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructAppUserDevice(repo);
					repo.AddAppUserDevice(entity);
					var items = repo.FetchAppUserDeviceAll();

					Assert.That(items, Is.Not.Null);
					Assert.That(items.Count, Is.GreaterThanOrEqualTo(1));
				});
		}

		/*
		[Test]
		[Category(Integration)]
		public void RemoveAppUserDeviceTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructAppUserDevice(repo);
					repo.AddAppUserDevice(entity);
					var readEntity = repo.FetchAppUserDeviceById(entity.AppUserDeviceId);
					Assert.That(readEntity, Is.Not.Null);
					repo.RemoveAppUserDevice(readEntity.AppUserDeviceId);
					readEntity = repo.FetchAppUserDeviceById(entity.AppUserDeviceId);
					Assert.That(readEntity, Is.Null);
				});
		}*/

		[Test]
		[Category(Integration)]
		public void InvalidIdReturnsNullAppUserDeviceTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var item = repo.FetchAppUserDeviceById(Guid.NewGuid());
					Assert.That(item, Is.Null);
				});
		}

		#endregion
	}
}