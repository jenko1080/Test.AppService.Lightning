# AppService TCP Listener Test

## Objectives

1. Test reliability of AppService for hosting a TCP subscription
   - Each disconnect and connect for the TCP service should be logged (allows for later analysis of continuity)
   - Data coming in should be logged... for some reason, maybe use?
1. Push data to a Service Bus... also for use...

## Local Set Up

### Requirements

1. Storage Account Table
   - Tables will be created as requested, at the time of writing `LightningStrokes`, `ConnectionLog` and `WarmupLog`
   - Defined in `Services/TablesService.cs`
1. Web PubSub Service
   - *NOTE*: Only Available in Australia East (Not South East)
1. ~~Service Bus (Replaced with PubSub... for now)~~
   1. ~~Topic: `lightning`~~
   1. ~~Subscription: `lightning-map`~~

### User Secrets

User secrets can be added to `AppSettings.json` or, preferably,  [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#secret-manager)

```ps
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:StorageTable" "<StorageConnectionString>"
dotnet user-secrets set "Lightning:Uri" "<LightningUri>"
dotnet user-secrets set "Lightning:Port" "<LightningPort>"
dotnet user-secrets set "Lightning:AuthString" "<LightningAuthString>"
dotnet user-secrets set "ConnectionStrings:PubSub" "<PubSubConnectionString>"

dotnet user-secrets set "ConnectionStrings:ServiceBus" "<ServiceBusConnectionString>"
```

- Note: Auth String expects the format of `WZLSF:CLIENTID:FORMAT:END`, ensure you use `JSON` as the format.
- Note: If doing local development, install `Azurite` and set StorageTable connection string to `UseDevelopmentStorage=true`

## Future TODO

- Update the PubSub implementation to send data every ?ms (batching)
- Update the Service Bus implementation to send data every ?ms (batching)
- Make the leaflet implementation more efficient?
