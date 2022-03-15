using Azure;
using Azure.Data.Tables;
using Test.AppService.Lightning.API.Exceptions;

namespace Test.AppService.Lightning.API.Models
{
    public class TableEntityBase : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public virtual void SetRowAndPartitionKeys()
        {
            throw new RowAndPartitionKeyNotImplementedException();
        }
    }
}
