using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using BusinessLogicLayer.Business.Framework;

namespace Tests
{
	public abstract class TestBase
	{
		private IPrincipal _currentPrincipal;
		//	private readonly SqlDatabase _defaultDatabase = new SqlDatabase(ConfigurationManager.ConnectionStrings[Constants.DatabaseConnectionStringName].ConnectionString); 

		/// <summary>
		/// 
		/// </summary>
		public const string Unit = "Unit";
		/// <summary>
		/// 
		/// </summary>
		public const string Integration = "Integration";

		/// <summary>
		/// 
		/// </summary>
		public const string Story = "Story";

		/// <summary>
		/// Called one-time for all methods that apply to all tests in a namespace or an assembly.
		/// </summary>
		[SetUp]
		public virtual void Setup()
		{
			// if you remove this line you will get nasty bs errors along the following lines
			/*
			 *
System.Runtime.Serialization.SerializationException : Type is not resolved for member 'Security.Principal.SecurityPrincipal.Common, Version=4.2.60.0, Culture=neutral, PublicKeyToken=null'.
   at System.AppDomain.GetHostEvidence(Type type)
   at System.Security.Policy.AppDomainEvidenceFactory.GenerateEvidence(Type evidenceType)
   at System.Security.Policy.Evidence.GenerateHostEvidence(Type type, Boolean hostCanGenerate)
   at System.Security.Policy.Evidence.GetHostEvidenceNoLock(Type type)
   at System.Security.Policy.Evidence.RawEvidenceEnumerator.MoveNext()
   at System.Security.Policy.Evidence.EvidenceEnumerator.MoveNext()
   at System.Configuration.ClientConfigPaths.GetEvidenceInfo(AppDomain appDomain, String exePath, String& typeName)
   at System.Configuration.ClientConfigPaths.GetTypeAndHashSuffix(AppDomain appDomain, String exePath)
   at System.Configuration.ClientConfigPaths..ctor(String exePath, Boolean includeUserConfig)
   at System.Configuration.ClientConfigPaths.GetPaths(String exePath, Boolean includeUserConfig)
   at System.Configuration.Internal.ConfigurationManagerInternal.System.Configuration.Internal.IConfigurationManagerInternal.get_ExeProductName()
   at System.Configuration.ApplicationSettingsBase.get_Initializer()
   at System.Configuration.ApplicationSettingsBase.CreateSetting(PropertyInfo propInfo)
   at System.Configuration.ApplicationSettingsBase.EnsureInitialized()
   at System.Configuration.ApplicationSettingsBase.get_Properties()
   at System.Configuration.SettingsBase.GetPropertyValueByName(String propertyName)
   at System.Configuration.SettingsBase.get_Item(String propertyName)
   at System.Configuration.ApplicationSettingsBase.GetPropertyValue(String propertyName)
   at System.Configuration.ApplicationSettingsBase.get_Item(String propertyName)
 


			 */
			//var manager = ObjectFactory.Resolve<IChangeTrackingManager>();
			//string fuckingBullshitThatIsNeededToRunTests = ConfigSettings.RedisHost;
			//JsConfig.ExcludeTypeInfo = true;
			//_currentPrincipal = SecurityPrincipal.GetSystem();
			//Thread.CurrentPrincipal = _currentPrincipal;
		}

		/// <summary>
		/// Called one-time for all methods that apply to all tests in a namespace or an assembly.
		/// </summary>
		/// <remarks></remarks>
		[TearDown]
		public virtual void TearDown()
		{
			Thread.CurrentPrincipal = _currentPrincipal;
		}


		/*
		protected int[] RequiredPermissions(string actionName, Type controllerType, Type argType = null)
		{
			return RequiredPermissions(actionName, controllerType, new[] { argType });
		}
		
		protected int[] RequiredPermissions(string actionName, Type controllerType, Type[] argType)
		{

			var methods = controllerType.GetMethods().Where(o => o.Name == actionName).ToArray();
			MethodInfo methodInfo = methods.FirstOrDefault();
			if (methods.Length > 1)
			{
				if (argType != null)
				{
					var possibleMethods = methods.Where(o => o.GetParameters().Length == argType.Length);
					foreach (MethodInfo possibleMethod in possibleMethods)
					{
						bool[] match = new bool[argType.Length];
						ParameterInfo[] parameterInfos = possibleMethod.GetParameters();
						for (int i = 0; i < parameterInfos.Length; i++)
						{
							match[i] = parameterInfos[i].ParameterType == argType[i];
						}
						if (!match.Contains(false))
							methodInfo = possibleMethod;
					}

				}
				else
				{
					methodInfo = methods.FirstOrDefault(o => o.GetParameters().Length == 0);
				}
			}
			var permissionAttributes = Attribute.GetCustomAttributes(methodInfo, typeof(PermissionAttribute)).OfType<PermissionAttribute>();
			List<int> permissions = new List<int>();
			foreach (PermissionAttribute attribute in permissionAttributes)
			{
				permissions.Add(attribute.Permission);
			}
			return permissions.ToArray();
		}*/


		protected IConfiguration Configuration
		{
			get => ConfigurationHelper.GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);
		}
		public static void InvokeInTransactionScope(Action<TransactionScope> action)
		{
			action.Invoke(null);
			/*
			TransactionScopeOption opt = TransactionScopeOption.Required;
			TransactionOptions options = new TransactionOptions();
			options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
			using (var scope = new TransactionScope(TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Suppress))
			{
				action.Invoke(scope);

			}
			*/
		}

		public static void InvokeInUncommitedTransactionScope(Action<TransactionScope> action)
		{
			using (TransactionScope scope = BusinessLogicComponent.TransactionScopeFactory.ConstructScope(TransactionScopeOption.RequiresNew, TimeSpan.FromSeconds(6), System.Transactions.IsolationLevel.ReadUncommitted))
			{
				action.Invoke(scope);
			}
		}
		public static int ExecuteNonQuery(SqlConnection connection, string commandText)
		{
			using (DbCommand command = connection.CreateCommand())
			{
				command.CommandText = commandText;
				command.CommandType = CommandType.Text;
				return command.ExecuteNonQuery();
			}
		}


		public static object ExecuteScalar(SqlConnection connection, string commandText)
		{
			using (DbCommand command = connection.CreateCommand())
			{
				command.CommandText = commandText;
				command.CommandType = CommandType.Text;
				return command.ExecuteScalar();
			}
		}

		//protected SqlDatabase DefaultDatabase
		//{
		//    get { return _defaultDatabase;  }
		//}
		/*
		protected static ControllerContext GetControllerContext(NameValueCollection form, string queryString)
		{
			Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
			mockRequest.Setup(r => r.Form).Returns(form ?? new NameValueCollection());

			NameValueCollection queryStringCollection = queryString != null ? HttpUtility.ParseQueryString(queryString) : new NameValueCollection();
			mockRequest.Setup(r => r.QueryString).Returns(queryStringCollection);

			Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
			mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

			return new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
		}*/

		public static string GetResourceFileContents(string filePath)
		{
			if (!filePath.StartsWith("Tests"))
				filePath = "Tests." + filePath;
			using (var stream = typeof(TestBase).Assembly.GetManifestResourceStream(filePath))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}

		protected void VerifyThatDatesAreCloseEnough(DateTime? actualDate, DateTime? expectedDate)
		{
			string message = string.Format("\r\n{0} ACTUAL\r\n{1} EXPECTED", actualDate, expectedDate);
			if (!actualDate.HasValue && !expectedDate.HasValue)
				return;
			if (actualDate.HasValue && !expectedDate.HasValue)
				Assert.Fail(message);
			if (!actualDate.HasValue && expectedDate.HasValue)
				Assert.Fail(message);
			DateTime actual = actualDate.Value;
			DateTime expected = expectedDate.Value;

			Assert.That(actual.Year, Is.EqualTo(expected.Year), "Years don't match " + message);
			Assert.That(actual.Month, Is.EqualTo(expected.Month), "Months don't match" + message);
			Assert.That(actual.Day, Is.EqualTo(expected.Day), "Days don't match" + message);
			Assert.That(actual.DayOfWeek, Is.EqualTo(expected.DayOfWeek), "" + message);
			Assert.That(actual.Hour, Is.EqualTo(expected.Hour), "Hours don't match" + message);
			Assert.That(actual.Minute, Is.EqualTo(expected.Minute), "Minutes don't match" + message);
			// 9/25/2015 MTV I am now allowing this stuff to be as much as 2 seconds off cause we keep having tests fail being off by a millisecond or so causing a trivial difference to fail the test.
			Assert.That(Math.Abs(actual.Second - expected.Second), Is.LessThanOrEqualTo(2), "Seconds don't match" + message);
			// it seems that when we interact with the db we lose 1 ms from orignal value to the value that is next retreived
			//Assert.That(actual.Millisecond, Is.EqualTo(expected.Millisecond), "Milliseconds don't match" + message);
		}


		/// <summary>
		/// Given a relative path, construct a full path name.
		/// Assumes the requested working directory is in the current path.
		/// </summary>
		/// <param name="workingDirectory"></param>
		/// <returns></returns>
		internal static string ConstructFullyQualifiedDirectory(string workingDirectory)
		{
			string current = Environment.CurrentDirectory;
			string fragment = new DirectoryInfo(current).Name;
			string relativeRoot = workingDirectory.Split(Path.DirectorySeparatorChar)[0];

			// now find the 
			while (fragment != relativeRoot)
			{
				current = Directory.GetParent(current).FullName;
				fragment = new DirectoryInfo(current).Name;
				if (Directory.GetDirectories(current).Any(x => new DirectoryInfo(x).Name == relativeRoot))
					break;
			}

			current = Directory.GetParent(current).FullName;
			return Path.Combine(current, fragment, workingDirectory);
		}

		public SqlException MakeSqlException()
		{
			SqlException exception = null;
			try
			{
				SqlConnection conn = new SqlConnection(@"Data Source=.;Database=GO_FIND_YOURSELF");
				conn.Open();
			}
			catch (SqlException ex)
			{
				exception = ex;
			}
			return (exception);
		}
	}
}
