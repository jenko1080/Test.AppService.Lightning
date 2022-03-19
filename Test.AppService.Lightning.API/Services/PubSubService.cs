using Azure.Core;
using Azure.Messaging.WebPubSub;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    public class PubSubService : IPubSubService
    {
        private readonly ILogger<PubSubService> _logger;
        private readonly WebPubSubServiceClient _webPubSubServiceClient;

        public PubSubService(ILogger<PubSubService> logger, WebPubSubServiceClient webPubSubServiceClient)
        {
            _logger = logger;
            _webPubSubServiceClient = webPubSubServiceClient;
        }

        public async Task<string> GetPubSubClientUriAsync()
        {
            var roles = new string[] {
                // "webpubsub.sendToGroup.stream", // - Clients only need to listen!
                "webpubsub.joinLeaveGroup.stream"
            };

            var clientUri = _webPubSubServiceClient.GetClientAccessUri(roles: roles).AbsoluteUri;

            return clientUri;
        }

        public async Task PublishMessageAsync(string payload)
        {
            _ = await _webPubSubServiceClient.SendToAllAsync(payload);
        }
    }
}
