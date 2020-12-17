using System.Collections.Generic;
using BusinessModel.Auth;
using BusinessModel.Auth.Model;

namespace BusinessLogicLayer.Business
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICentralAuthManager
	{
	/// <summary>
	/// Used to log on to the system
	/// </summary>
	/// <param name="authRequest"></param>
	/// <returns></returns>
	AppUser LogOn(AuthRequest authRequest);

	/// <summary>
	/// Used to create a new user
	/// </summary>
	/// <param name="appUser"></param>
	void CreateAppUser(AppUser appUser);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="appUserId"></param>
	/// <param name="username"></param>
	/// <param name="newPassword"></param>
	void ChangePassword(long appUserId, string username, string newPassword);

	/// <summary>
	/// Used to check if a username is in use
	/// </summary>
	/// <param name="username"></param>
	/// <returns></returns>
	bool CheckUsernameAppUser(string username);

	/// <summary>
	/// Returns a list of clients that a given user is allowed to access
	/// </summary>
	/// <param name="appUserId"></param>
	/// <returns></returns>
	List<AppUserClientAccess> FetchClientAccessForAppUser(long appUserId);

	/// <summary>
	/// Returns a list of clients that the current user is allowed to access
	/// </summary>
	/// <returns></returns>
	List<AppUserClientAccess> FetchClientAccessForMe();
	}
}