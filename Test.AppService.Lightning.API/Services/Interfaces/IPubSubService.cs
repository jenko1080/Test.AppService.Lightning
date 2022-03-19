namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface IPubSubService
    {
        Task<string> GetPubSubClientUriAsync();
        Task PublishMessageAsync(string payload);
    }
}