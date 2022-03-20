using Azure;
using Azure.Data.Tables;
using System.Text.Json.Serialization;
using Test.AppService.Lightning.API.Exceptions;

namespace Test.AppService.Lightning.API.Models
{
    public class TableEntityBase : ITableEntity
    {
        [JsonIgnore]
        public string PartitionKey { get; set; }
        [JsonIgnore]
        public string RowKey { get; set; }
        [JsonIgnore]
        public DateTimeOffset? Timestamp { get; set; }
        [JsonIgnore]
        public ETag ETag { get; set; }

        public virtual void SetRowAndPartitionKeys()
        {
            throw new RowAndPartitionKeyNotImplementedException();
        }
    }
}
