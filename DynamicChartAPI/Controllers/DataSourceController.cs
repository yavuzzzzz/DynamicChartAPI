using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DynamicChartAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DataSourceController : ControllerBase
	{
		[HttpPost("LoadData")]
		public IActionResult LoadData([FromBody] DataSourceRequest request)
		{
			string connectionString = "Server=MONSTER-OF-YAVUZ\\SQLEXPRESS;Database=DbChart;Integrated Security=True;TrustServerCertificate=True;";

			try
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();

					SqlCommand command = new SqlCommand();
					command.Connection = connection;

					
					switch (request.DataSourceType.ToLower())
					{
						case "function":
							if (request.DataSourceName.Equals("GetTotalSalesInDateRange", StringComparison.OrdinalIgnoreCase))
							{
								
								command.CommandText = $"SELECT * FROM {request.DataSourceName}(@StartDate, @EndDate)";
								command.CommandType = CommandType.Text;
								command.Parameters.AddWithValue("@StartDate", DateTime.Parse("2024-01-01"));
								command.Parameters.AddWithValue("@EndDate", DateTime.Parse("2024-01-31"));
							}
							else
							{
								return BadRequest(new { message = "Invalid function name" });
							}
							break;
						case "stored_procedure":
							if (request.DataSourceName.Equals("GetSalesByProduct", StringComparison.OrdinalIgnoreCase))
							{
								
								command.CommandText = request.DataSourceName;
								command.CommandType = CommandType.StoredProcedure;
								command.Parameters.AddWithValue("@ProductName", "Product A");
							}
							else
							{
								return BadRequest(new { message = "Invalid stored procedure name" });
							}
							break;
						case "view":
							if (request.DataSourceName.Equals("vw_ProductSalesSummary", StringComparison.OrdinalIgnoreCase))
							{
								command.CommandText = $"SELECT * FROM {request.DataSourceName}";
								command.CommandType = CommandType.Text;
							}
							else
							{
								return BadRequest(new { message = "Invalid view name" });
							}
							break;
						default:
							return BadRequest(new { message = "Invalid data source type" });
					}

					using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
					{
						DataTable dataTable = new DataTable();
						dataAdapter.Fill(dataTable);

						return Ok(dataTable);
					}
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Failed to load data", error = ex.Message });
			}
		}
	}
}

public class DataSourceRequest
{
	public string DataSourceType { get; set; } // function, stored_procedure, view
	public string DataSourceName { get; set; } // Veri kaynağının adı
}
