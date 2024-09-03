using DynamicChartAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DataContext to the dependency injection container
builder.Services.AddSingleton<DataContext>();  // or AddScoped<DataContext>() based on your needs

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
	//app.UseSwaggerUI(c =>
	//{
	//	c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
	//	c.RoutePrefix = string.Empty;  // Swagger UI'yi kök URL'de çalýþtýrýr
	//});

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
