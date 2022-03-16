namespace Test.AppService.Lightning.API.Models
{
    public class ConnectionLogEntry : TableEntityBase
    {
        public DateTime DateTimeUtc { get; set; }

        public ConnectionUpdateStatus NewStatus { get; set; }

        public string? UpdateMessage { get; set; }

        public int? Number { get; set; }

        public override void SetRowAndPartitionKeys()
        {
            PartitionKey = DateTimeUtc.ToString("yyyy-MM-dd");
            RowKey = DateTimeUtc.ToString("HH:mm:ss.fff");
        }
    }
}
