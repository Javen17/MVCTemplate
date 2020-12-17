using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogicLayer.Business.Framework;
using BusinessLogicLayer.Data.Central;
using BusinessLogicLayer.Security;

namespace BusinessLogicLayer.Business
{
	public class ClientDatabaseConnectionProvider : BusinessLogicComponent, IClientDatabaseConnectionProvider
	{
		private readonly ICentralAuthRepository _authRepository;
		private readonly object _mutex = new object();
		private readonly Dictionary<long, string> _connectionStrings = new Dictionary<long, string>();
		public ClientDatabaseConnectionProvider(ICentralAuthRepository authRepository, ICommonPrincipleAccessor principleAccessor)
		: base(principleAccessor)
		{

		}

		public string GetConnectionString()
		{
			long activeClientId = CurrentPrincipal.CommonIdentity.ActiveClientId;
			lock (_mutex)
			{
				if (!_connectionStrings.ContainsKey(activeClientId))
				{
					
				}

				return _connectionStrings[activeClientId];
			}
		}
	}
}
