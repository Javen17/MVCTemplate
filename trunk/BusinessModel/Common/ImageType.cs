using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;
using Common.Utilities;

namespace BusinessModel.Common
{
	/// <summary>
	/// Enumeration of the support attachment formats
	/// </summary>
	public enum ImageType
	{
		/// <summary>
		/// JPG image format
		/// </summary>
		[EnumMember(Value = "jpg")]
		Jpeg = 1,

		/// <summary>
		/// PNG image format
		/// </summary>
		[EnumMember(Value = "png")]
		Png = 2,

		/// <summary>
		/// animated gif
		/// </summary>
		[EnumMember(Value = "gif")]
		Gif = 3,

		/// <summary>
		/// PDF type
		/// </summary>
		[EnumMember(Value = "jpg")]
		Pdf = 4
	}
}