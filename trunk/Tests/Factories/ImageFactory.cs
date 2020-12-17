using System;
using BusinessModel.Common;

namespace Tests.Factories
{
	public static class ImageFactory
	{
		public static Image CreateImage()
		{
			return new Image
			{
				FileId = Guid.NewGuid(),
				CreatedDate = DateTime.Now,
				IsActive = true,
				Name = Guid.NewGuid().ToString(),
				Type = ImageType.Jpeg,
				Url = $"https://domain.com/{Guid.NewGuid()}/{Guid.NewGuid()}.jpg"
			};
		}
	}
}
