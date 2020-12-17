using System.ComponentModel.DataAnnotations;

namespace BusinessModel.Auth
{
	/// <summary>
	/// Describes what access a user has to give clients
	/// </summary>
	public class AppUserClientAccess
	{
		/// <summary>
		/// Gets or sets ClientUserId
		/// </summary>
		[Required]
		public long ClientUserId { get; set; }

		/// <summary>
		/// Gets or sets ClientId
		/// </summary>
		[Required]
		public long ClientId { get; set; }

		/// <summary>
		/// Gets or sets AppRoleId
		/// </summary>
		[Required]
		public short AppRoleId { get; set; }

		/// <summary>
		/// Gets or sets ClientName
		/// </summary>
		[Required]
		[StringLength(100)]
		public string ClientName { get; set; }

		/// <summary>
		/// Gets or sets InternalRoleName
		/// </summary>
		[Required]
		[StringLength(20)]
		public string InternalRoleName { get; set; }
	}
}