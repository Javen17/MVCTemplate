using System.Collections.Generic;
using BusinessModel.Common;

namespace BusinessLogicLayer.Data.Client
{
	/// <summary>
	/// Defines the shared repo
	/// </summary>
	public interface ISharedRepository
	{
		#region Internationalization

		/// <summary>
		/// Used to add an instance of <see cref="Internationalization"/>
		/// </summary>
		/// <param name="internationalization">The instance of <see cref="Internationalization" /> that you want to create</param>
		void AddInternationalization(Internationalization internationalization);

		/// <summary>
		/// Used to modify an instance of <see cref="Internationalization"/>
		/// </summary>
		/// <param name="internationalization">The instance of <see cref="Internationalization" /> that you want to modify</param>
		void ModifyInternationalization(Internationalization internationalization);

		/// <summary>
		/// Used to add an instance of  <see cref="Internationalization"/>
		/// </summary>
		/// <param name="internationalizationId">the primary key for the Internationalization you want to remove</param>
		void RemoveInternationalization(int internationalizationId);

		/// <summary>
		/// Used to find an instance of <see cref="Internationalization"/> by passing in its primary key
		/// </summary>
		/// <param name="internationalizationId"></param>
		/// <returns>The instance of Internationalization that matches the Key or null if not found</returns>
		Internationalization FetchInternationalizationById(int internationalizationId);

		/// <summary> 
		/// Used to return all instances of <see cref="Internationalization"/> that exists
		/// </summary>
		/// <returns>A collection of all the Internationalizations in the system</returns>
		List<Internationalization> FetchInternationalizationAll();


		#endregion end of Internationalization

		#region AppLanguage

		/// <summary>
		/// Used to add an instance of <see cref="AppLanguage"/>
		/// </summary>
		/// <param name="appLanguage">The instance of <see cref="AppLanguage" /> that you want to create</param>
		void AddAppLanguage(AppLanguage appLanguage);

		/// <summary>
		/// Used to modify an instance of <see cref="AppLanguage"/>
		/// </summary>
		/// <param name="appLanguage">The instance of <see cref="AppLanguage" /> that you want to modify</param>
		void ModifyAppLanguage(AppLanguage appLanguage);

		/// <summary>
		/// Used to add an instance of  <see cref="AppLanguage"/>
		/// </summary>
		/// <param name="appLanguageId">the primary key for the AppLanguage you want to remove</param>
		void RemoveAppLanguage(byte appLanguageId);

		/// <summary>
		/// Used to find an instance of <see cref="AppLanguage"/> by passing in its primary key
		/// </summary>
		/// <param name="appLanguageId"></param>
		/// <returns>The instance of AppLanguage that matches the Key or null if not found</returns>
		AppLanguage FetchAppLanguageById(byte appLanguageId);

		/// <summary> 
		/// Used to return all instances of <see cref="AppLanguage"/> that exists
		/// </summary>
		/// <returns>A collection of all the AppLanguages in the system</returns>
		List<AppLanguage> FetchAppLanguageAll();

		#endregion end of AppLanguage
	}
}