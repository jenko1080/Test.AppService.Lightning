using Azure.Messaging.ServiceBus;
using System.Text.Json;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    public class TopicService : ITopicService
    {
        private const string _lightningTopicName = "lightning";
        private readonly ILogger<TopicService> _logger;
        private readonly ServiceBusClient _serviceBusClient;

        public TopicService(ILogger<TopicService> logger, ServiceBusClient serviceBusClient)
        {
            _logger = logger;
            _serviceBusClient = serviceBusClient;
        }

        // TODO: Consider collecting over 1 second and batching to reduce load?
        public async Task AddLightningMessage(LightningStrokeEntry stroke)
        {
            try
            {
                // Create a message sender
                var sender = _serviceBusClient.CreateSender(_lightningTopicName);
                // Serialise the lightning data
                var message = new ServiceBusMessage(JsonSerializer.Serialize(stroke));
                // Send the message
                await sender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Attempt to send service bus message recieved an unexpected errpr: {ex.ToString}");
            }
        }
    }
}
