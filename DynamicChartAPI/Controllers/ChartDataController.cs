using DynamicChartAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace DynamicChartAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChartDataController : ControllerBase
	{
		private readonly DataContext _context;

		public ChartDataController(DataContext context)
		{
			_context = context;
		}

		[HttpGet("GetSalesByProduct")]
		public IActionResult GetSalesByProduct(string productName)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter("@ProductName", productName)
			};

			DataTable dt = _context.ExecuteStoredProcedure("GetSalesByProduct", parameters);
			string jsonResult = JsonConvert.SerializeObject(dt);

			return Ok(jsonResult);
		}

		[HttpGet("GetTotalSalesInDateRange")]
		public IActionResult GetTotalSalesInDateRange(string startDate, string endDate)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter("@StartDate", startDate),
				new SqlParameter("@EndDate", endDate)
			};

			DataTable dt = _context.ExecuteFunction("GetTotalSalesInDateRange", parameters);
			string jsonResult = JsonConvert.SerializeObject(dt);

			return Ok(jsonResult);
		}

		[HttpGet("GetProductSalesSummary")]
		public IActionResult GetProductSalesSummary()
		{
			DataTable dt = _context.ExecuteView("vw_ProductSalesSummary");
			string jsonResult = JsonConvert.SerializeObject(dt);

			return Ok(jsonResult);
		}
	}
}
