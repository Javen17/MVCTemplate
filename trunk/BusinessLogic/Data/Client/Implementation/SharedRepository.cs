using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using BusinessLogicLayer.Data.Framework;
using BusinessModel.Common;

namespace BusinessLogicLayer.Data.Client.Implementation
{
	/// <inheritdoc cref="ISharedRepository"/> />
	public class SharedRepository : DataAccessComponent, ISharedRepository
	{
		/// <inheritdoc />
		public SharedRepository(IConfiguration configuration) : base(configuration, false)
		{
		}


		/// <inheritdoc />
		public void AddAppLanguage(AppLanguage appLanguage)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.pAppLanguage_Add");
			AddParameterToCommand(command, "@AppLanguageId", DbType.Byte, appLanguage.AppLanguageId, 1);
			AddParameterToCommand(command, "@Name", DbType.String, appLanguage.Name, 100);
			AddParameterToCommand(command, "@CultureString", DbType.String, appLanguage.CultureString, 10);
			command.ExecuteNonQuery();
			appLanguage.AppLanguageId = (byte)GetParameterValue(command, "@AppLanguageId");
		}

		/// <inheritdoc />
		public void ModifyAppLanguage(AppLanguage appLanguage)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.pAppLanguage_Modify");
			AddParameterToCommand(command, "@AppLanguageId", DbType.Byte, appLanguage.AppLanguageId, 1);
			AddParameterToCommand(command, "@Name", DbType.String, appLanguage.Name, 100);
			AddParameterToCommand(command, "@CultureString", DbType.String, appLanguage.CultureString, 10);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public void RemoveAppLanguage(byte appLanguageId)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.AppLanguage_Remove");
			AddParameterToCommand(command, "@AppLanguageId", DbType.Byte, appLanguageId);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public AppLanguage FetchAppLanguageById(byte appLanguageId)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.pAppLanguage_Fetch");
			AddParameterToCommand(command, "@AppLanguageId", DbType.Byte, appLanguageId);
			using IDataReader reader = command.ExecuteReader();
			if (!reader.Read())
				return null;
			return Factory.ConstructAppLanguage(reader);
		}

		/// <inheritdoc />
		public List<AppLanguage> FetchAppLanguageAll()
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.pAppLanguage_Fetch");
			AddParameterToCommand(command, "@AppLanguageId", DbType.Byte, 0);
			using IDataReader reader = command.ExecuteReader();
			List<AppLanguage> entities = new List<AppLanguage>();
			while (reader.Read())
			{
				entities.Add(Factory.ConstructAppLanguage(reader));
			}
			return entities;
		}

		/// <inheritdoc />
		public void AddInternationalization(Internationalization internationalization)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.pInternationalization_Add");
			AddParameterToCommand(command, "@InternationalizationId", DbType.Int32, internationalization.InternationalizationId, 4);
			AddParameterToCommand(command, "@AppLanguageId", DbType.Byte, internationalization.AppLanguageId, 1);
			AddParameterToCommand(command, "@Name", DbType.String, internationalization.Name, 120);
			AddParameterToCommand(command, "@Text", DbType.String, internationalization.Text, -1);
			AddParameterToCommand(command, "@ReferenceTypeId", DbType.Int16, internationalization.ReferenceTypeId, 2);
			AddParameterToCommand(command, "@ReferenceEntityId", DbType.Int64, internationalization.ReferenceEntityId, 8);
			command.ExecuteNonQuery();
			internationalization.InternationalizationId = (int)GetParameterValue(command, "@InternationalizationId");
		}

		/// <inheritdoc />
		public void ModifyInternationalization(Internationalization internationalization)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.pInternationalization_Modify");
			AddParameterToCommand(command, "@InternationalizationId", DbType.Int32, internationalization.InternationalizationId, 4);
			AddParameterToCommand(command, "@AppLanguageId", DbType.Byte, internationalization.AppLanguageId, 1);
			AddParameterToCommand(command, "@Name", DbType.String, internationalization.Name, 120);
			AddParameterToCommand(command, "@Text", DbType.String, internationalization.Text, -1);
			AddParameterToCommand(command, "@ReferenceTypeId", DbType.Int16, internationalization.ReferenceTypeId, 2);
			AddParameterToCommand(command, "@ReferenceEntityId", DbType.Int64, internationalization.ReferenceEntityId, 8);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public void RemoveInternationalization(int internationalizationId)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.Internationalization_Remove");
			AddParameterToCommand(command, "@InternationalizationId", DbType.Int32, internationalizationId);
			command.ExecuteNonQuery();
		}

		/// <inheritdoc />
		public Internationalization FetchInternationalizationById(int internationalizationId)
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.pInternationalization_Fetch");
			AddParameterToCommand(command, "@InternationalizationId", DbType.Int32, internationalizationId);
			using IDataReader reader = command.ExecuteReader();
			if (!reader.Read())
				return null;
			return Factory.ConstructInternationalization(reader);
		}
		/// <inheritdoc />
		public List<Internationalization> FetchInternationalizationAll()
		{
			using var connection = CreateConnection();
			using var command = CreateCommand(connection, "Shared.pInternationalization_Fetch");
			AddParameterToCommand(command, "@InternationalizationId", DbType.Int32, 0);
			using IDataReader reader = command.ExecuteReader();
			List<Internationalization> entities = new List<Internationalization>();
			while (reader.Read())
			{
				entities.Add(Factory.ConstructInternationalization(reader));
			}
			return entities;
		}

		public static class Factory
		{
			public static Internationalization ConstructInternationalization(IDataReader reader)
			{
				Internationalization entity = new Internationalization();
				entity.InternationalizationId = DataReaderUtility.GetValue<int>(reader, "InternationalizationId");
				entity.AppLanguageId = DataReaderUtility.GetValue<byte>(reader, "AppLanguageId");
				entity.Name = DataReaderUtility.GetValue<string>(reader, "Name");
				entity.Text = DataReaderUtility.GetValue<string>(reader, "Text");
				entity.ReferenceTypeId = DataReaderUtility.GetValue<short?>(reader, "ReferenceTypeId");
				entity.ReferenceEntityId = DataReaderUtility.GetValue<long?>(reader, "ReferenceEntityId");
				return entity;
			}

			public static AppLanguage ConstructAppLanguage(IDataReader reader)
			{
				AppLanguage entity = new AppLanguage();
				entity.AppLanguageId = DataReaderUtility.GetValue<byte>(reader, "AppLanguageId");
				entity.Name = DataReaderUtility.GetValue<string>(reader, "Name");
				entity.CultureString = DataReaderUtility.GetValue<string>(reader, "CultureString");
				return entity;
			}

		}
	}
}