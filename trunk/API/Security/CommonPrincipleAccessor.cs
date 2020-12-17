using BusinessLogicLayer.Security;
using Common.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Security
{
	public class CommonPrincipleAccessor : ICommonPrincipleAccessor
	{
		private readonly IHttpContextAccessor _HttpContextAccessor;
		public CommonPrincipleAccessor(IHttpContextAccessor httpContextAccessor)
		{
			_HttpContextAccessor = httpContextAccessor;
		}

		/// <inheritdoc />
		public CommonPrincipal CurrentPrincipal => _HttpContextAccessor.HttpContext.User as CommonPrincipal;
	}
}
