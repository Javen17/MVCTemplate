using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessModel.Auth
{
	/// <summary>
	/// 
	/// </summary>
	public class AppUserDevice
	{
		/// <summary>
		/// Gets or sets AppUserDeviceId
		/// </summary>
		public Guid AppUserDeviceId { get; set; }

		/// <summary>
		/// Gets or sets AppUserId
		/// </summary>
		[JsonIgnore]
		public long AppUserId { get; set; }

		/// <summary>
		/// Gets or sets LastLoginDate
		/// </summary>
		[JsonIgnore]
		public DateTime LastLoginDate { get; set; }

		/// <summary>
		/// Gets or sets LastIp
		/// </summary>
		[JsonIgnore]
		[StringLength(20)]
		public string LastIp { get; set; }

		/// <summary>
		/// Gets or sets DeviceToken
		/// </summary>
		[Required]
		[StringLength(200)]
		public string DeviceToken { get; set; }

		/// <summary>
		/// Gets or sets IsBlocked
		/// </summary>
		[JsonIgnore]
		public bool IsBlocked { get; set; }

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

		/// <summary>
		/// Gets or sets IsTrusted
		/// </summary>
		public bool IsTrusted { get; set; }
	}
}