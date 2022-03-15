using Azure;
using Azure.Data.Tables;
using Test.AppService.Lightning.API.Exceptions;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    public class TablesService : ITablesService
    {
        private ILogger _logger;
        private TableServiceClient _tableServiceClient;

        // Create lookup dictionary for model name to table name
        private static readonly Dictionary<string, string> _modelToTableName = new Dictionary<string, string>()
        {
            { nameof(LightningStrokeEntry), "LightningStrokes" },
            { nameof(ConnectionLogEntry), "ConnectionLog" }
        };

        // Ctor
        public TablesService(ILogger<TablesService> logger, TableServiceClient tableClient)
        {
            _logger = logger;
            _tableServiceClient = tableClient;
        }

        // Add generic object to table
        public async Task<bool> AddToTable<T>(T entity) where T : TableEntityBase
        {
            try
            {
                // Get table name from model name
                string tableName = _modelToTableName.GetValueOrDefault(typeof(T).Name) ?? throw new KeyNotFoundException();

                // Set partition key and row key
                entity.SetRowAndPartitionKeys();

                // Get table client for tableName
                var tableClient = _tableServiceClient.GetTableClient(tableName);

                // Create table if it doesn't exist
                await tableClient.CreateIfNotExistsAsync();

                // Create table entity
                await tableClient.AddEntityAsync(entity);
                return true;
            }
            catch (RequestFailedException e)
            {
                _logger.LogError($"Error adding to table: {e.Message}");
            }
            catch (KeyNotFoundException)
            {
                _logger.LogError($"No table mapping available for type {typeof(T).Name}");
            }
            catch (RowAndPartitionKeyNotImplementedException)
            {
                _logger.LogError($"Row and partition key not implemented for type {typeof(T).Name}");
            }

            return false;
        }
    }
}
