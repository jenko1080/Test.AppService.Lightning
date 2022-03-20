using Microsoft.Extensions.Azure;
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

// Get connection strings from storage
var serviceBusConnectionString = builder.Configuration.GetConnectionString("ServiceBus");
var storageConnectionString = builder.Configuration.GetConnectionString("StorageTable");
var pubSubConnectionString = builder.Configuration.GetConnectionString("PubSub");

// Set up the Table Service and Service Bus client services
builder.Services.AddAzureClients(builder =>
{
    // Longer term logging and data storage
    builder.AddTableServiceClient(storageConnectionString);
    // Potentially for a reliable stream for multiple subscribers
    // - Not yet needed, but for other apps that want to do something for every item
    builder.AddServiceBusClient(serviceBusConnectionString);
    // Pub sub for web clients that just want to view the data
    builder.AddWebPubSubServiceClient(pubSubConnectionString, "stream");
});

// Set up Service Bus service
builder.Services.AddSingleton<ITopicService, TopicService>();

// Set up table service
builder.Services.AddSingleton<ITablesService, TablesService>();

// Add lightning services
builder.Services.AddSingleton<ILightningService, LightningService>();
builder.Services.AddSingleton<ITcpWorkerService, TcpWorkerService>();
builder.Services.AddSingleton<IWarmupService, WarmupService>();
builder.Services.AddSingleton<IPubSubService, PubSubService>();
builder.Services.AddTransient<IConfigService, ConfigService>();

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
