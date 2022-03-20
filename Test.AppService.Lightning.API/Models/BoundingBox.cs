using Microsoft.Spatial;

namespace Test.AppService.Lightning.API.Models
{
    public class BoundingBox
    {
        public GeographyPoint BottomLeft { get; set; }
        public GeographyPoint TopRight { get; set; }
    }
}
