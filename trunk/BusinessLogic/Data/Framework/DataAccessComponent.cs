using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using IsolationLevel = System.Data.IsolationLevel;

namespace BusinessLogicLayer.Data.Framework
{
	/// <summary>
	/// core of how we access data
	/// </summary>
	public class DataAccessComponent : IDisposable
	{
		public static string TestDirectory { get; set; }
		private static IConfigurationRoot BuildConfiguration() 
		{
			return new ConfigurationBuilder()
				.SetBasePath(TestDirectory)
				
				.AddJsonFile("appsettings.json", optional: true)
				.Build();
		}
		private static readonly int _commandTimeout = 60;
		private readonly string _connectionString;
		private readonly bool _useCentral;
		/// <summary>
		/// Initializes the <see cref="DataAccessComponent"/> class.
		/// </summary>
		/// <remarks></remarks>
		static DataAccessComponent()
		{
			/*if (false == int.TryParse(.AppSettings["FoxHorn.Data.CommandTimeout"], out _commandTimeout))
			{
				_commandTimeout = 60;
			}
			*/
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataAccessComponent"/> class.
		/// </summary>
		protected DataAccessComponent(IConfiguration configuration, bool useCentral)
			: this(TestHelper.IsTesting ?  TestHelper.TestConnectionString : configuration.GetConnectionString("DbConnectionString"))
		{
			_useCentral = useCentral;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataAccessComponent"/> class.
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		protected DataAccessComponent(string connectionStringName, bool useRaw = false)
		{
			_connectionString = connectionStringName;
			Console.WriteLine(_connectionString);
			DbConnection = CreateConnection();
			if (DbConnection.State != ConnectionState.Open)
				DbConnection.Open();
		}
		private IDbTransaction ForcedTransaction { get; set; }

		public IDbTransaction BeginTransaction()
		{
			ForcedTransaction = DbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
			return ForcedTransaction;
		}

		public void CommitTransaction()
		{
			ForcedTransaction.Commit();
		}

		/// <summary>
		/// Gets the command timeout.
		/// </summary>
		/// <remarks></remarks>
		protected int CommandTimeout
		{
			get { return _commandTimeout; }
		}

		public SqlConnection CreateConnection()
		{
			return new SqlConnection(_connectionString);
		}

		public IDbConnection DbConnection { get; }

		public IDbCommand GetStoredProcCommandFromActiveConnection(string sprocName)
		{

			var command = DbConnection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = sprocName;
			return command;
		}

		protected DbCommand CreateCommand(DbConnection connection, string commandName)
		{
			if (connection.State != ConnectionState.Open)
				connection.Open();

			var command = connection.CreateCommand();
			command.CommandText = commandName;
			command.CommandType = CommandType.StoredProcedure;
			return command;
		}


		protected DbCommand CreateRawCommand(DbConnection connection, string commandText)
		{
			if (connection.State != ConnectionState.Open)
				connection.Open();

			var command = connection.CreateCommand();
			command.CommandText = commandText;
			command.CommandType = CommandType.Text;
			return command;
		}

		public void AddParameterToCommand(IDbCommand command, string name, DbType paramType, object value, int? size = null)
		{
			var param = command.CreateParameter();
			param.DbType = paramType;

			param.ParameterName = name;
			param.Value = value ?? DBNull.Value;
			if (size.HasValue)
				param.Size = size.Value;
			command.Parameters.Add(param);
		}

		public void AddOutputParameterToCommand(IDbCommand command, string name, DbType paramType, int? size = null)
		{
			var param = command.CreateParameter();
			param.DbType = paramType;
			param.ParameterName = name;
			param.Direction = ParameterDirection.Output;
			if (size.HasValue)
				param.Size = size.Value;
			command.Parameters.Add(param);
		}

		protected object GetParameterValue(IDbCommand command, string parameterName)
		{
			return ((SqlParameter)command.Parameters[parameterName]).Value;
		}

		protected T GetParameterValue<T>(IDbCommand command, string parameterName)
		{
			var param = ((SqlParameter)command.Parameters[parameterName]);
			if (param.Value is System.DBNull)
				return default(T);
			return (T)((SqlParameter)command.Parameters[parameterName]).Value;
		}

		public static int ExecuteNonQuery(SqlConnection conn, string cmdText, SqlParameter[] cmdParms)
		{
			SqlCommand cmd = conn.CreateCommand();
			PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			SqlCommand cmd = conn.CreateCommand();
			using (conn)
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				int val = cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				return val;
			}
		}

		public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			SqlCommand cmd = conn.CreateCommand();
			PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
			var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			return rdr;
		}

		// DataTable only available in .NET CORE 2.0 preview or later
		// 2017-05-13: sqldataadapter just put into builds YESTERDAY, not in any releases - https://github.com/dotnet/corefx/pull/19682/files/422ee5fcd9aa6f97b348fe278af634f1ff2c694e
		//  have to use datatables the hard way
		public static DataTable ExecuteDataTable(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			DataTable dt = new DataTable();
			// just doing this cause dr.load fails
			dt.Columns.Add("CustomerID");
			dt.Columns.Add("CustomerName");
			SqlDataReader dr = ExecuteReader(conn, cmdType, cmdText, cmdParms);
			// as of now dr.Load throws a big nasty exception saying its not supported. wip.
			// dt.Load(dr);
			while (dr.Read())
			{
				dt.Rows.Add(dr[0], dr[1]);
			}
			return dt;
		}

		public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			SqlCommand cmd = conn.CreateCommand();
			PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
			object val = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			return val;
		}

		protected void ExecuteNonQuery(IDbCommand command, IEnumerable<SqlParameter> commandParameters)
		{
			if (DbConnection.State != ConnectionState.Open)
			{
				DbConnection.Open();
			}
			if (ForcedTransaction != null)
			{
				command.Transaction = ForcedTransaction;
			}
			//attach the command parameters if they are provided
			if (commandParameters != null)
			{
				foreach (SqlParameter p in commandParameters)
				{
					//check for derived output value with no value assigned
					if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
					{
						p.Value = DBNull.Value;
					}
					command.Parameters.Add(p);
				}
			}
			command.ExecuteNonQuery();
		}

		protected IDataReader ExecuteReader(IDbCommand command, IEnumerable<SqlParameter> commandParameters)
		{
			if (DbConnection.State != ConnectionState.Open)
			{
				DbConnection.Open();
			}
			if (ForcedTransaction != null)
			{
				command.Transaction = ForcedTransaction;
			}
			//attach the command parameters if they are provided
			if (commandParameters != null)
			{
				foreach (SqlParameter p in commandParameters)
				{
					//check for derived output value with no value assigned
					if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
					{
						p.Value = DBNull.Value;
					}
					command.Parameters.Add(p);
				}
			}
			return command.ExecuteReader();
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		protected static T GetValue<T>(IDataReader reader, string columnName)
		{
			/*
			Condition.Requires(reader, "reader").IsNotNull();
			Condition.Requires(columnName, "columnName").IsNotNullOrEmpty();
			*/
			//if (reader.IsClosed)
			//{
			//    return default(T);
			//}

			int index = reader.GetOrdinal(columnName);
			if (reader.IsDBNull(index))
			{
				return default(T);
			}
			return (T)reader.GetValue(index);
		}

		protected static void PrepareCommand(SqlCommand cmd, SqlConnection conn, IDbTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters)
		{
			if (conn.State != ConnectionState.Open)
			{
				conn.Open();
			}
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
			{
				//cmd.Transaction = trans;
			}
			cmd.CommandType = cmdType;
			//attach the command parameters if they are provided
			if (commandParameters != null)
			{
				AttachParameters(cmd, commandParameters);
			}
		}
		private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
		{

			foreach (SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}
				command.Parameters.Add(p);
			}
		}

		public void Dispose()
		{
			ForcedTransaction?.Dispose();
			if (DbConnection.State == ConnectionState.Open)
				DbConnection?.Dispose();
		}
	}
}