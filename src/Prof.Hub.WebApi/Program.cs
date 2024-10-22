using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prof.Hub.Application;
using Prof.Hub.Infrastructure;
using Prof.Hub.Infrastructure.ApiClients.Configurations;
using Prof.Hub.Infrastructure.PostgresSql;
using Prof.Hub.Infrastructure.PostgresSql.Configurations;
using Prof.Hub.WebApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.Configure<PostgreSqlSettings>(
    builder.Configuration.GetSection("PostgreSqlSettings"));

builder.Services.Configure<ExternalServicesSettings>(
    builder.Configuration.GetSection("ExternalServices"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("RedisSettings")["ConnectionString"];
});

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var postgresSettings = serviceProvider.GetRequiredService<IOptions<PostgreSqlSettings>>().Value;
    options.UseNpgsql(postgresSettings.ConnectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseHsts();

app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger().UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
