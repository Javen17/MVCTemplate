using BusinessLogicLayer.Business;
using BusinessModel.Auth;
using BusinessModel.Auth.Model;
using BusinessModel.Security;
using BusinessModel.Validation.Auth;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
	namespace ZafiPOS.Api.Controllers
	{
		/// <summary>
		/// 
		/// </summary>
		[ApiController]
		[Route("auth")]
		public class AuthController : BaseController
		{
			private readonly ICentralAuthManager _centralAuthManager;
			/// <summary>
			/// 
			/// </summary>
			/// <param name="authManager"></param>
			public AuthController(ICentralAuthManager centralAuthManager)
			{
				_centralAuthManager = centralAuthManager;
			}


			/// <summary>
			/// Used to log on and get a token to take further actions with the api.
			/// </summary>
			/// <param name="model"></param>
			/// <returns></returns>
			[HttpPost("log-on")]
			[Produces("application/json")]
			[ProducesResponseType(200, Type = typeof(AppTokenInfo))]
			[ProducesResponseType(400, Type = typeof(LogOnFailed))]
			[ProducesResponseType(401, Type = typeof(AppTokenInfo))]
			public async Task<object> Login([FromBody] AuthRequest model)
			{

				var failedModel = new LogOnFailed();
				try
				{

					IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
					IJsonSerializer serializer = new JsonNetSerializer();
					IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
					IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

					var user = _centralAuthManager.LogOn(model);

					if (user == null)
					{
						return new AppTokenInfo { Token = "fail", Username = "guest", UserId = Guid.Empty };
					}


					string token = string.Empty;

					token = encoder.Encode(IdentityFactory.AppUserToClaimsFactory(user), Constants.Secret);
					return new AppTokenInfo
					{
						Token = token,
						Username = user.Username,
						UserId = user.ExternalId
					};
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					return e.ToString();

				}
			}

			/// <summary>
			/// user to verify that the token you have is good.
			/// </summary>
			/// <returns></returns>
			[Produces("application/json")]
			[HttpGet("verify")]
			public object VerifyToken()
			{
				string[] parts = HttpContext.Request.Headers["Authorization"][0].Split(' ');

				var json = new JwtBuilder()
					.WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
					.WithSecret(Constants.Secret)
					.MustVerifySignature()
					.Decode(parts[1]);
				return json;
			}
		}
	}
}
