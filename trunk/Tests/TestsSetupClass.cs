using System;
using BusinessLogicLayer.Data.Framework;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Tests
{
	/// <summary>
	/// setup and clean up after our tests
	/// </summary>
	[SetUpFixture]
	public class TestsSetupClass
	{
		private static readonly string[] BeforeTestQueries = new string[]
		{

		};



		[OneTimeSetUp]
		public void GlobalSetup()
		{
			//DataAccessComponent.TestDirectory = TestContext.CurrentContext.TestDirectory;
			new UnfuckTestRepository(null).RunPreTestQueries();
		}

		[OneTimeTearDown]
		public void GlobalTeardown()
		{
			//new UnfuckTestRepository(null).RunPostTestQueries();
		}

		public class UnfuckTestRepository : DataAccessComponent
		{
			public UnfuckTestRepository(IConfiguration configuration)
				: base(configuration,true)
			{

			}

			public void RunPreTestQueries()
			{
				Console.WriteLine("Starting pre test clean up now.");
				for (int i = 0; i < BeforeTestQueries.Length; i++)
				{
					using (var connection = CreateConnection())
					{
						using (var command = CreateRawCommand(connection, BeforeTestQueries[i]))
						{
							Console.WriteLine($"   Starting command '{BeforeTestQueries[i]}'.");
							command.ExecuteNonQuery();
							Console.WriteLine($"   Finished command '{BeforeTestQueries[i]}'.");
						}
					}
				}
				Console.WriteLine("Finished pre test clean up.");

			}

			public void RunPostTestQueries()
			{
				Console.WriteLine("Starting post test clean up now.");
				for (int i = 0; i < AfterTestQueries.Length; i++)
				{
					using (var connection = CreateConnection())
					{
						using (var command = CreateRawCommand(connection, AfterTestQueries[i]))
						{
							Console.WriteLine($"   Starting command '{AfterTestQueries[i]}'.");
							command.ExecuteNonQuery();
							Console.WriteLine($"   Finished command '{AfterTestQueries[i]}'.");
						}
					}
				}
				Console.WriteLine("Finished post test clean up.");
			}
		}

		private static readonly string[] AfterTestQueries = new string[]
		{
			"TRUNCATE TABLE Auth.AppUserPasswordReset",
			"DELETE FROM Auth.AppUserDevice",
			"DELETE FROM Auth.AppUser WHERE AppUserId >= 100000;DBCC CHECKIDENT ('Auth.AppUser', RESEED, 100000);",
			"DELETE FROM Auth.AppRole WHERE AppRoleId >= 500;DBCC CHECKIDENT ('Auth.AppRole', RESEED, 500);",
			"DELETE FROM Shared.Image WHERE ImageId >= 100000;DBCC CHECKIDENT ('Shared.Image', RESEED, 100000);",
			/*
			"TRUNCATE TABLE Auth.AppUserPasswordReset",
			"DELETE FROM Sales.Product WHERE ProductId >= 100000;DBCC CHECKIDENT ('Sales.Product', RESEED, 100000);",
			"DELETE FROM Sales.ClientBrandLocation WHERE ClientBrandLocationId >= 100000;DBCC CHECKIDENT ('Sales.ClientBrandLocation', RESEED, 100000);",
			"DELETE FROM Sales.ClientBrand WHERE ClientBrandId >= 100000;DBCC CHECKIDENT ('Sales.ClientBrand', RESEED, 100000);",
			"DELETE FROM Sales.ClientUser WHERE ClientUserId >= 100000;DBCC CHECKIDENT ('Sales.ClientUser', RESEED, 100000);",
			"DELETE FROM Sales.ClientMembershipLevel WHERE ClientMembershipLevelId >= 100000;DBCC CHECKIDENT ('Sales.ClientMembershipLevel', RESEED, 100000);",
			"DELETE FROM Shared.Image WHERE ImageId >= 100000;DBCC CHECKIDENT ('Shared.Image', RESEED, 100000);",
			"DELETE FROM Sales.Client WHERE ClientId >= 100000;DBCC CHECKIDENT ('Sales.Client', RESEED, 100000);",
			"DELETE FROM Auth.AppUserDevice",
			"DELETE FROM Auth.AppUser WHERE AppUserId >= 100000;DBCC CHECKIDENT ('Auth.AppUser', RESEED, 100000);",
			"DELETE FROM Auth.AppRole WHERE AppRoleId >= 500;DBCC CHECKIDENT ('Auth.AppRole', RESEED, 500);",
			
			"DELETE FROM Shared.GlobalBrand WHERE GlobalBrandId >= 100000;DBCC CHECKIDENT ('Shared.GlobalBrand', RESEED, 100000);",
			"DELETE FROM Shared.GlobalProductCategory WHERE GlobalProductCategoryId >= 100000;DBCC CHECKIDENT ('Shared.GlobalProductCategory', RESEED, 100000);",
			"DELETE FROM Sales.MembershipLevel WHERE MembershipLevelId >= 100000;DBCC CHECKIDENT ('Sales.MembershipLevel', RESEED, 100000);",
			*/
		};
	}
}