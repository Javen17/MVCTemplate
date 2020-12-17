using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using BusinessLogicLayer.Data.Client;
using BusinessLogicLayer.Data.Framework;
using BusinessModel.Common;

namespace BusinessLogicLayer.Data.Client.Implementation
{
    /// <summary>
    /// Used to persist images
    /// </summary>
    public class ImageRepository : DataAccessComponent, IImageRepository
    {
        /// <inheritdoc />
        public void AddImage(Image image)
        {
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, "Shared.pImage_Add");
            AddOutputParameterToCommand(command, "@ImageId", DbType.Int64, 8);
            AddParameterToCommand(command, "@ClientId", DbType.Int64, image.ClientId, 8);
            AddParameterToCommand(command, "@Name", DbType.String, image.Name, 100);
            AddParameterToCommand(command, "@TypeId", DbType.Int16, image.TypeId, 2);
            AddParameterToCommand(command, "@Url", DbType.String, image.Url, 200);
            AddParameterToCommand(command, "@IsActive", DbType.Boolean, image.IsActive, 1);
            AddParameterToCommand(command, "@FileId", DbType.Guid, image.FileId, 36);
            AddParameterToCommand(command, "@CreatedDate", DbType.DateTime, image.CreatedDate, 8);
            command.ExecuteNonQuery();
            image.ImageId = GetParameterValue<long>(command, "@ImageId");
        }

        /// <inheritdoc />
        public void ModifyImage(Image image)
        {
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, "Shared.pImage_Modify");
            AddParameterToCommand(command, "@ImageId", DbType.Int64, image.ImageId, 8);
            AddParameterToCommand(command, "@Name", DbType.String, image.Name, 100);
            AddParameterToCommand(command, "@IsActive", DbType.Boolean, image.IsActive, 1);
            command.ExecuteNonQuery();
        }

        /// <inheritdoc />
        public void RemoveImage(long imageId)
        {
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, "Shared.pImage_Remove");
            AddParameterToCommand(command, "@ImageId", DbType.Int64, imageId);
            command.ExecuteNonQuery();
        }

        /// <inheritdoc />
        public Image FetchImageById(long imageId)
        {
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, "Shared.pImage_Fetch");
            AddParameterToCommand(command, "@ImageId", DbType.Int64, imageId);
            using IDataReader reader = command.ExecuteReader();
            if (!reader.Read())
                return null;
            return Factory.ConstructImage(reader);
        }

        /// <inheritdoc />
        public List<Image> FetchImageAll()
        {
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, "Shared.pImage_Fetch");
            AddParameterToCommand(command, "@ImageId", DbType.Int64, 0);
            using IDataReader reader = command.ExecuteReader();
            List<Image> entities = new List<Image>();
            while (reader.Read())
            {
                entities.Add(Factory.ConstructImage(reader));
            }

            return entities;
        }


        public static class Factory
        {
            public static Image ConstructImage(IDataReader reader)
            {
                Image entity = new Image();
                entity.ImageId = DataReaderUtility.GetValue<long>(reader, "ImageId");
                entity.ClientId = DataReaderUtility.GetValue<long>(reader, "ClientId");
                entity.Name = DataReaderUtility.GetValue<string>(reader, "Name");
                entity.TypeId = DataReaderUtility.GetValue<short>(reader, "TypeId");
                entity.Url = DataReaderUtility.GetValue<string>(reader, "Url");
                entity.IsActive = DataReaderUtility.GetValue<bool>(reader, "IsActive");
                entity.FileId = DataReaderUtility.GetValue<Guid>(reader, "FileId");
                entity.CreatedDate = DataReaderUtility.GetValue<DateTime>(reader, "CreatedDate");
                return entity;
            }
        }

		/// <inheritdoc />
		public ImageRepository(IConfiguration configuration)
			: base(configuration,false)
		{
		}
	}
}