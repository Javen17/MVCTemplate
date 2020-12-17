using System.Collections.Generic;
using BusinessModel.Common;

namespace BusinessLogicLayer.Data.Client
{
	/// <summary>
	/// Defines the interface to image persistence
	/// </summary>
	public interface IImageRepository
	{
		#region Image

		/// <summary>
		/// Used to add an instance of <see cref="Image"/>
		/// </summary>
		/// <param name="image">The instance of <see cref="Image" /> that you want to create</param>
		void AddImage(Image image);

		/// <summary>
		/// Used to modify an instance of <see cref="Image"/>
		/// </summary>
		/// <param name="image">The instance of <see cref="Image" /> that you want to modify</param>
		void ModifyImage(Image image);

		/// <summary>
		/// Used to add an instance of  <see cref="Image"/>
		/// </summary>
		/// <param name="imageId">the primary key for the Image you want to remove</param>
		void RemoveImage(long imageId);

		/// <summary>
		/// Used to find an instance of <see cref="Image"/> by passing in its primary key
		/// </summary>
		/// <param name="imageId"></param>
		/// <returns>The instance of Image that matches the Key or null if not found</returns>
		Image FetchImageById(long imageId);

		/// <summary> 
		/// Used to return all instances of <see cref="Image"/> that exists
		/// </summary>
		/// <returns>A collection of all the Images in the system</returns>
		List<Image> FetchImageAll();

		#endregion end of Image
	}
}