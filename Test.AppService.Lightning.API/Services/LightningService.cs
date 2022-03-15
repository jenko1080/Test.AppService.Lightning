using System.Text.Json;
using System.Text.Json.Serialization;
using Test.AppService.Lightning.API.JsonConverters;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    public class LightningService : ILightningService
    {
        private readonly ILogger<LightningService> _logger;
        private readonly ITablesService _tablesService;

        // Ctor
        public LightningService(ILogger<LightningService> logger, ITablesService tablesService)
        {
            _logger = logger;
            _tablesService = tablesService;
        }

        // Handle JSON lightning stroke
        // schema: {"ts":"2022-03-15 03:27:54.376","lat":-8.99178,"lon":162.95541,"kamp":29.169,"st":"CG","km":0.0,"sen":10,"pul":1};
        public async Task HandleLightningJson(string lightning)
        {
            // Parse JSON
            try
            {
                lightning = lightning.TrimEnd(';');

                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new DateTimeConverter());
                options.Converters.Add(new JsonStringEnumConverter());

                var lightningStroke = JsonSerializer.Deserialize<LightningStrokeEntry>(lightning, options);

                if (lightningStroke == null)
                {
                    _logger.LogError("Failed to deserialise Lightning payload");
                    return;
                }

                // If KeepAlive is recieved, log it
                if(lightningStroke.Type == LightningStrokeType.KA)
                {
                    _logger.LogInformation("Lightning feed KeepAlive message recieved");
                    return;
                }

                // Ignore JH messages
                if(lightningStroke.Type == LightningStrokeType.JH)
                {
                    return;
                }
                
                // Save to table
                if (await _tablesService.AddToTable(lightningStroke))
                {
                    _logger.LogDebug($"Lightning added to table");
                }
                else
                {
                    _logger.LogWarning("Lightning failed to add to table");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Json Deserialize failed: {ex.ToString} {ex.Message}");
            }
            catch(Exception ex)
            {
                _logger.LogError(JsonSerializer.Serialize(lightning));
                throw;
            }
        }

    }
}
