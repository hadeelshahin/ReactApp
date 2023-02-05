using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(j =>
{
	j.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

var app = builder.Build();

// Configure the HTTP request pipeline.


app.MapControllers();

	app.Run();
