using Azure.Messaging.WebPubSub;
using System.Text.Json;
using System.Text.Json.Serialization;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    public class PubSubService : IPubSubService
    {
        private readonly ILogger<PubSubService> _logger;
        private readonly WebPubSubServiceClient _webPubSubServiceClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public PubSubService(ILogger<PubSubService> logger, WebPubSubServiceClient webPubSubServiceClient)
        {
            _logger = logger;
            _webPubSubServiceClient = webPubSubServiceClient;

            // Serializer options for sending data to frontend web apps
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = {
                    // Use strings to save having to pass enum definition
                    new JsonStringEnumConverter()
                }
            };
        }

        public async Task<string> GetPubSubClientUriAsync()
        {
            var roles = new string[] {
                // "webpubsub.sendToGroup.stream", // - Clients only need to listen!
                "webpubsub.joinLeaveGroup.stream"
            };

            var clientUri = (await _webPubSubServiceClient.GetClientAccessUriAsync(roles: roles)).AbsoluteUri;

            return clientUri;
        }

        public async Task PublishLightningMessageAsync(LightningStrokeEntry lightning)
        {
            var message = JsonSerializer.Serialize(lightning, _jsonSerializerOptions);

            _ = await _webPubSubServiceClient.SendToAllAsync(message);
        }

        public async Task PublishKeepAliveMessageAsync(KeepAliveMessage keepAlive)
        {
            var message = JsonSerializer.Serialize(keepAlive, _jsonSerializerOptions);

            _ = await _webPubSubServiceClient.SendToAllAsync(message);
        }
    }
}
