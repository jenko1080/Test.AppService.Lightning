using Test.AppService.Lightning.API.Enums;

namespace Test.AppService.Lightning.API.Models
{
    public class WarmupEntry : TableEntityBase
    {
        public DateTime DateTimeUtc { get; set; }

        public WarmupStatus Status { get; set; }

        public override void SetRowAndPartitionKeys()
        {
            PartitionKey = DateTimeUtc.ToString("yyyy-MM-dd");
            RowKey = DateTimeUtc.ToString("HH:mm:ss.fff");
        }
    }
}
