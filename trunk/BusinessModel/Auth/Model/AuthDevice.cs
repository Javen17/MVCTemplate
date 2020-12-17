using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessModel.Auth.Model
{
	/// <summary>
	/// The Device model for authentication
	/// </summary>
	public class AuthDevice
	{
		/// <summary>
		/// Gets or sets AppUserDeviceId
		/// </summary>
		public Guid AppUserDeviceId { get; set; }

		/// <summary>
		/// Gets or sets DeviceToken
		/// </summary>
		[Required]
		[StringLength(200)]
		public string DeviceToken { get; set; }


		/// <summary>
		/// Gets or sets DevicePlatformId
		/// </summary>
		[Required]
		public byte DevicePlatformId { get; set; }

		/// <summary>
		/// Gets or sets DeviceModel
		/// </summary>
		[Required]
		[StringLength(30)]
		public string DeviceModel { get; set; }

		/// <summary>
		/// Gets or sets OsVersion
		/// </summary>
		[Required]
		[StringLength(10)]
		public string OsVersion { get; set; }

		/// <summary>
		/// Gets or sets PushNotificationToken
		/// </summary>
		[StringLength(255)]
		public string PushNotificationToken { get; set; }

		public static implicit operator AppUserDevice(AuthDevice model)
		{
			if (model == null)
				return null;
			return new AppUserDevice
			{
				AppUserDeviceId = model.AppUserDeviceId,
				DeviceModel = model.DeviceModel,
				DevicePlatformId = model.DevicePlatformId,
				DeviceToken = model.DeviceToken,
				OsVersion = model.OsVersion,
				PushNotificationToken = model.PushNotificationToken
			};
		}
	}
}