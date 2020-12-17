using System.ComponentModel.DataAnnotations;

namespace BusinessModel.Common
{
	public class Internationalization
	{
		[Required]
		public int InternationalizationId { get; set; }

		[Required]
		public byte AppLanguageId { get; set; }

		[Required]
		[StringLength(120)]
		public string Name { get; set; }

		[Required]
		[StringLength(4096)]
		public string Text { get; set; }

		public short? ReferenceTypeId { get; set; }

		public long? ReferenceEntityId { get; set; }
	}
}