using BusinessLogicLayer.Business;
using BusinessModel.Auth;
using BusinessModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    /// <summary>
    /// used for user related actions 
    /// </summary>
    [ApiController]
    [Route("app-user")]
    public class AppUserController : BaseController
    {
        private readonly ICentralAuthManager _centralAuthManager;
        public AppUserController(ICentralAuthManager centralAuthManager)
        {
            _centralAuthManager = centralAuthManager;
        }

        /// <summary>
        /// Used to create a user
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        [HttpPost("register", Name = "RegisterAppUser")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(AppUser))]
        public AppUser Register(AppUser appUser)
        {
            _centralAuthManager.CreateAppUser(appUser);
            return appUser;
        }

        /// <summary>
        /// Used to see if a username can be used
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("check/username/{username}", Name = "CheckUsername")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(OperationResponse))]
        public OperationResponse CheckIfUsernameIsInUse(string username)
        {
            var reply = new OperationResponse();
            try
            {
                if (_centralAuthManager.CheckUsernameAppUser(username))
                {
                    reply.Result = false;
                    reply.Message = "Username already taken";
                }
                else
                {
                    reply.Result = true;
                }
            }
            catch (Exception e)
            {
                reply.Message = e.ToString();
            }

            return reply;
        }



        /// <summary>
        /// Used to see fetch clients that the user has access to
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("client-access", Name = "FetchClientAccess")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(AppUserClientAccess[]))]
        public AppUserClientAccess[] FetchClientAccess()
        {
            return _centralAuthManager.FetchClientAccessForMe().ToArray();
        }
    }
}

