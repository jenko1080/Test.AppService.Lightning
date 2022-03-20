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
        private readonly ITopicService _topicService;
        private readonly IPubSubService _pubSubService;
        private readonly IConfiguration _configuration;
        private readonly IConfigService _configService;

        // Ctor
        public LightningService(ILogger<LightningService> logger, ITablesService tablesService, ITopicService topicService, IPubSubService pubSubService, IConfiguration configuration, IConfigService configService)
        {
            _logger = logger;
            _tablesService = tablesService;
            _topicService = topicService;
            _pubSubService = pubSubService;
            _configuration = configuration;
            _configService = configService;
        }

        // Handle JSON lightning stroke
        // schema: {"ts":"2022-03-15 03:27:54.376","lat":-8.99178,"lon":162.95541,"kamp":29.169,"st":"CG","km":0.0,"sen":10,"pul":1};
        public async Task HandleLightningJson(string lightning)
        {
            // Parse JSON
            try
            {
                var lightningStroke = DeserialiseLightning(lightning);

                if (lightningStroke == null)
                {
                    _logger.LogError("Failed to deserialise Lightning payload");
                    return;
                }

                // If KeepAlive is recieved, log it
                if (lightningStroke.Type == LightningStrokeType.KA)
                {
                    _logger.LogInformation("Lightning feed KeepAlive message recieved");

                    // Construct new slim KeepAlive
                    var keepAlive = new KeepAliveMessage
                    {
                        DateTimeUtc = lightningStroke.DateTimeUtc,
                        Type = lightningStroke.Type,
                    };

                    // Add to PubSub
                    await _pubSubService.PublishKeepAliveMessageAsync(keepAlive);

                    return;
                }

                // Save to table
                if (IsInBoundingBox(lightningStroke))
                {
                    //await _topicService.AddLightningMessage(lightningStroke);
                    _logger.LogDebug("Lightning stroke is in Victoria bounding box: {lat}, {lon}", lightningStroke.Latitude, lightningStroke.Longitude);

                    // Add to PubSub
                    await _pubSubService.PublishLightningMessageAsync(lightningStroke);

                    // Log to long term table
                    if (await _tablesService.AddToTable(lightningStroke))
                    {
                        _logger.LogDebug($"Lightning added to table");
                    }
                    else
                    {
                        _logger.LogWarning("Lightning failed to add to table");
                    }
                }
                else
                {
                    _logger.LogDebug($"Lightning ignored due to bounding box: {lightningStroke.Latitude}, {lightningStroke.Longitude}");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Json Deserialize failed: {ex.ToString} {ex.Message}");
            }
            catch(Exception)
            {
                _logger.LogError(JsonSerializer.Serialize(lightning));
                throw;
            }
        }

        public bool IsConnectionMessage(string lightning)
        {
            var lightningStroke = DeserialiseLightning(lightning);
            
            // Ignore JH messages
            if(lightningStroke?.Type == LightningStrokeType.JH)
            {
                return true;
            }

            return false;
        }

        private static LightningStrokeEntry? DeserialiseLightning(string lightning)
        {
            lightning = lightning.TrimEnd(';');

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new DateTimeConverter());
            options.Converters.Add(new JsonStringEnumConverter());

            var lightningStroke = JsonSerializer.Deserialize<LightningStrokeEntry>(lightning, options);
            return lightningStroke;
        }

        /// <summary>
        /// Checks if strike is within bounding box (defaults to Victoria if lat/lon aren't configured)
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        private bool IsInBoundingBox(LightningStrokeEntry l)
        {
            var box = _configService.GetBoundingBox();

            return l.Latitude > box.BottomLeft.Latitude
                && l.Latitude < box.TopRight.Longitude
                && l.Longitude > box.BottomLeft.Latitude
                && l.Longitude < box.TopRight.Longitude ? true : false;
        }

    }
}
