using Microsoft.Data.SqlClient;
using System.Data;

namespace DynamicChartAPI.Data
{
	public class DataContext
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;

		public DataContext(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("DefaultConnection");
		}

		// Stored Procedure Execution
		public DataTable ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(procedureName, conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					if (parameters != null)
					{
						cmd.Parameters.AddRange(parameters);
					}

					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						da.Fill(dt);
						return dt;
					}
				}
			}
		}

		// Function Execution
		public DataTable ExecuteFunction(string functionName, SqlParameter[] parameters)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;

					// Fonksiyonun çağrılma şeklini oluştur
					string parameterList = string.Join(", ", parameters.Select(p => p.ParameterName));
					cmd.CommandText = $"SELECT * FROM {functionName}({parameterList})";

					foreach (var param in parameters)
					{
						cmd.Parameters.Add(param);
					}

					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						da.Fill(dt);
						return dt;
					}
				}
			}
		}

		// View Execution
		public DataTable ExecuteView(string viewName)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {viewName}", conn))
				{
					cmd.CommandType = CommandType.Text;

					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						da.Fill(dt);
						return dt;
					}
				}
			}
		}
	}
}
