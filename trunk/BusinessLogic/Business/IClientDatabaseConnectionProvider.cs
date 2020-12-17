using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Business
{
	/// <summary>
	/// Defines how to access the client database based on the current user
	/// </summary>
	public interface IClientDatabaseConnectionProvider
	{
		/// <summary>
		/// Used to get the connection string for client data access for the current user
		/// </summary>
		/// <returns></returns>
		string GetConnectionString();
	}
}
