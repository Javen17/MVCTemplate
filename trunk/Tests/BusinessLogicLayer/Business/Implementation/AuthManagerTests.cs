using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using BusinessLogicLayer.Business;
using BusinessLogicLayer.Business.Implementation;
using BusinessLogicLayer.Data.Central.Implementation;
using BusinessModel.Auth.Model;
using Common;
using Tests.Factories.Security;

namespace Tests.BusinessLogicLayer.Business.Implementation
{
	[TestFixture]
	public class CentralAuthManagerTests : TestBase
	{
		protected ICentralAuthManager GetManager()
		{
			return new CentralAuthManager(new CentralAuthRepository(null), null, null);
		}

		[Test]
		[Category(Unit)]
		public void VerifyPasswordHashingIsNotFucked()
		{
			string key = ":)";
			string username = Guid.NewGuid().ToString();
			string password = Guid.NewGuid().ToString();
			string hash = PasswordStorage.CreateHash(username + password + key);


			Assert.True(PasswordStorage.VerifyPassword(username + password + key, hash));
		}

		[Test]
		[Category(Integration)]
		public void VerifyCanCreateAppUserThenLoginWithItTest()
		{
			var appUser = AuthFactory.CreateAppUser();
			var target = GetManager();
			string password = appUser.Password;
			Console.WriteLine(password);
			target.CreateAppUser(appUser);
			Assert.That(appUser.ExternalId, Is.Not.EqualTo(Guid.Empty));
			Assert.That(appUser.AppUserId, Is.GreaterThan(0));
			// verify that these properties are cleared so that they can't some how be sent over the wire.
			Assert.That(appUser.PasswordHash, Is.Null);
			Assert.That(appUser.Password, Is.Null);

			var request = new AuthRequest
			{
				Username = appUser.Username,
				Password = password,
				Device = new AuthDevice()
			};
			var logOnAppUser = target.LogOn(request);
			Assert.That(logOnAppUser, Is.Not.Null);
		}


		[Test]
		[Category(Integration)]
		public void VerifyChangePasswordWorksAsExpectedTest()
		{
			var appUser = AuthFactory.CreateAppUser();
			var target = GetManager();
			string password = appUser.Password;
			target.CreateAppUser(appUser);
			// verify that these properties are cleared so that they can't some how be sent over the wire.

			var request = new AuthRequest
			{
				Username = appUser.Username,
				Password = password,
				Device = new AuthDevice()
			};
			// verify that it can be logged into with the original password
			var logOnAppUser = target.LogOn(request);
			Assert.That(logOnAppUser, Is.Not.Null);
			string newPassword = Guid.NewGuid().ToString();

			// change the password
			target.ChangePassword(appUser.AppUserId, appUser.Username, newPassword);

			// verify the old password doesn't still work 
			request.Password = password;
			logOnAppUser = target.LogOn(request);
			Assert.That(logOnAppUser, Is.Null);

			// verify the new password does work
			request.Password = newPassword;
			logOnAppUser = target.LogOn(request);
			Assert.That(logOnAppUser, Is.Not.Null);
		}
	}
}