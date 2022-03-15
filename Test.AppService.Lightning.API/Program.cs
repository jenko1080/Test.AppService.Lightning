using Azure.Data.Tables;
using Microsoft.Extensions.Logging.AzureAppServices;
using Test.AppService.Lightning.API.Services;
using Test.AppService.Lightning.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add the Azure logger configurations
// CASE: Log to file system is enabled (via App Service Logs - only single instance)
builder.Services.Configure<AzureFileLoggerOptions>(options =>
{
    options.FileName = "azure-diagnostics-";
    options.FileSizeLimit = 50 * 1024;
    options.RetainedFileCountLimit = 5;
});
// CASE: Log to blob storage is enabled
builder.Services.Configure<AzureBlobLoggerOptions>(options =>
{
    options.BlobName = "log.txt";
});

// Add logging provider to write to app service
builder.Logging.AddAzureWebAppDiagnostics();
// Add app insights telemetry services
builder.Services.AddApplicationInsightsTelemetry();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set up table service
var connectionString = builder.Configuration.GetConnectionString("StorageTable");
builder.Services.AddSingleton<TableServiceClient>(new TableServiceClient(connectionString));
builder.Services.AddSingleton<ITablesService, TablesService>();

// Add lightning services
builder.Services.AddSingleton<ILightningService, LightningService>();
builder.Services.AddSingleton<ITcpWorkerService, TcpWorkerService>();

// Add background service
builder.Services.AddHostedService<TcpBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
