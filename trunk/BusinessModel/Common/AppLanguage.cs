using System.ComponentModel.DataAnnotations;

namespace BusinessModel.Common
{
	public class AppLanguage
	{
		[Required]
		public byte AppLanguageId { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		[StringLength(10)]
		public string CultureString { get; set; }
	}
}