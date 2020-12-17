using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Common.Security
{
	/// <summary>
	/// 
	/// </summary>
	public class CommonPrincipal : ClaimsPrincipal
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="identity"></param>
		public CommonPrincipal(CommonIdentity identity)
		: base(identity)
		{
			CommonIdentity = identity;

		}


		/// <summary>
		/// The id 
		/// </summary>
		public CommonIdentity CommonIdentity { get; }
	}
}