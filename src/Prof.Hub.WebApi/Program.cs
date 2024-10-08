using Prof.Hub.Application.Extensions;
using Prof.Hub.Infrastructure;
using Prof.Hub.Infrastructure.ApiClients.Configurations;
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

builder.Services.Configure<ExternalServicesSettings>(
    builder.Configuration.GetSection("ExternalServices"));

var app = builder.Build();

app.UseHsts();

app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseSwagger().UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
