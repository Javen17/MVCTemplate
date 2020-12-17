using System;
using System.Security.Principal;

namespace BusinessModel.Security
{
	public class SecurityPrinciple : IPrincipal
	{
		public bool IsInRole(string role)
		{
			throw new NotImplementedException();
		}

		public IIdentity? Identity { get; }
	}
}