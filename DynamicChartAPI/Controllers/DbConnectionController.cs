using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;

namespace DynamicChartAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DbConnectionController : ControllerBase
	{
		[HttpPost("Connect")]
		public IActionResult Connect([FromBody] DbConnectionRequest request)
		{
			// Dinamik olarak bağlantı dizesini oluştur, Windows Authentication kullan
			var connectionString = $"Server={request.ServerName};Database={request.DbName};Integrated Security=True;TrustServerCertificate=True;";

			try
			{
				using (var connection = new SqlConnection(connectionString))
				{
					connection.Open();  // Bağlantıyı dene

					// Bağlantı başarılı
					return Ok(new { message = "Connection successful!" });
				}
			}
			catch (Exception ex)
			{
				// Bağlantı başarısız
				return BadRequest(new { message = "Connection failed!", error = ex.Message });
			}
		}
	}

	public class DbConnectionRequest
	{
		public string ServerName { get; set; }
		public string DbName { get; set; }
	}
}
