using NUnit.Framework;
using BusinessLogicLayer.Data;
using BusinessLogicLayer.Data.Client;
using BusinessLogicLayer.Data.Client.Implementation;
using BusinessModel.Common;
using Tests.Factories;

namespace Tests.BusinessLogicLayer.Data.Implementation
{
	public class ImageRepositoryTests : TestBase
	{
		#region Image Tests

		private Image ConstructImage(IImageRepository repo)
		{
			var entity = ImageFactory.CreateImage();

			// todo: build out your entity dependencies here
			return entity;
		}

		[Test]
		[Category(Integration)]
		public void CreateImageTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructImage(repo);
					Assert.That(entity.ImageId, Is.EqualTo(0));
					repo.AddImage(entity);
					Assert.That(entity.ImageId, Is.Not.EqualTo(0));
				});
		}

		private IImageRepository GetRepository()
		{
			return new ImageRepository(null);
		}

		[Test]
		[Category(Integration)]
		public void ModifyImageTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructImage(repo);
					repo.AddImage(entity);
					entity.Name = "changed name";

					entity.IsActive = !entity.IsActive;
					repo.ModifyImage(entity);
					var readImage = repo.FetchImageById(entity.ImageId);
					Assert.That(readImage.ImageId, Is.EqualTo(entity.ImageId));
					Assert.That(readImage.Name, Is.EqualTo(entity.Name));
					Assert.That(readImage.IsActive, Is.EqualTo(entity.IsActive));
				});
		}

		[Test]
		[Category(Integration)]
		public void FetchImageAllTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructImage(repo);
					repo.AddImage(entity);
					var items = repo.FetchImageAll();

					Assert.That(items, Is.Not.Null);
					Assert.That(items.Count, Is.GreaterThanOrEqualTo(1));
				});
		}

		[Test]
		[Category(Integration)]
		public void RemoveImageTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var entity = ConstructImage(repo);
					repo.AddImage(entity);
					var readEntity = repo.FetchImageById(entity.ImageId);
					Assert.That(readEntity, Is.Not.Null);
					repo.RemoveImage(readEntity.ImageId);
					readEntity = repo.FetchImageById(entity.ImageId);
					Assert.That(readEntity, Is.Null);
				});
		}

		[Test]
		[Category(Integration)]
		public void InvalidIdReturnsNullImageTest()
		{
			InvokeInTransactionScope(
				s =>
				{
					var repo = GetRepository();
					var item = repo.FetchImageById(short.MaxValue);
					Assert.That(item, Is.Null);
				});
		}

		#endregion
	}
}