using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessModel.Common
{
	/// <summary>
	/// 
	/// </summary>
	public class Image
	{
		public const string Endpoint = "https://playerapp.blob.core.windows.net/images";
		public const string ThumbnailPrefix = "t";
		public const string FullSizePrefix = "f";
		public const string PathFormat = "images/{0}/{1}/{2}_{3}.jpg";

		/// <summary>
		/// Gets or sets ImageId
		/// </summary>
		[Required]
		public long ImageId { get; set; }

		/// <summary>
		/// Gets or sets ClientId
		/// </summary>
		[JsonIgnore]
		public long ClientId { get; set; }

		/// <summary>
		/// Gets or sets Name
		/// </summary>
		[StringLength(100)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets TypeId
		/// </summary>
		[JsonIgnore]
		public short TypeId { get; set; }

		/// <summary>
		/// Gets or sets TypeId
		/// </summary>
		[Required]
		public ImageType Type
		{
			get => (ImageType)TypeId;
			set => TypeId = (short)value;
		}

		/// <summary>
		/// Gets or sets Url
		/// </summary>

		[StringLength(200)]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets IsActive
		/// </summary>
		[Required]
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets FileId
		/// </summary>
		[JsonIgnore]
		public Guid FileId { get; set; }

		/// <summary>
		/// Gets or sets CreatedDate
		/// </summary>
		[JsonIgnore]
		public DateTime CreatedDate { get; set; }

		public string Message { get; set; }
		public string SourcePath { get; set; }

		public string RawData { get; set; }

		public string FullPath => string.Format(TypeId == 0 ? PathFormat : PathFormat.Replace(".jpg", GetExtension(Type)), CreatedDate.Year, CreatedDate.Month, FullSizePrefix, FileId);
		public string ThumbnailPath => string.Format(TypeId == 0 ? PathFormat : PathFormat.Replace(".jpg", GetExtension(Type)), CreatedDate.Year, CreatedDate.Month, ThumbnailPrefix, FileId);
		public string ContainerName => $"images/{ClientId}/{CreatedDate.Year}/{CreatedDate.Month}/";
		public string AbsoluteThumbnailPath => $"{Endpoint}/{ClientId}/{CreatedDate.Year}/{CreatedDate.Month}/{ThumbnailPrefix}_{FileId}" + GetExtension(Type);

		public string AbsoluteFullPath => $"{Endpoint}/{ClientId}/{CreatedDate.Year}/{CreatedDate.Month}/{FullSizePrefix}_{FileId}" + GetExtension(Type);

		public static string GetContentType(ImageType type)
		{
			switch (type)
			{
				case ImageType.Jpeg:
					return "image/jpg";
				case ImageType.Png:
					return "image/png";
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public static string GetExtension(ImageType type)
		{
			switch (type)
			{
				case ImageType.Png:
					return ".png";
				default:
					return ".jpg";
			}
		}

		public static ImageType GetContentType(string type)
		{
			switch (type)
			{
				case "image/jpg":
				case "image/jpeg":
					return ImageType.Jpeg;
				case "image/png":
					return ImageType.Png;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}
