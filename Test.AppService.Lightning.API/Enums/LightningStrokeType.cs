// enum for lightning stroke type of IC or CG
namespace Test.AppService.Lightning.API.Models
{
    public enum LightningStrokeType
    {
        CG = 1,
        IC,
        KA, // KeepAlive record (every 15 seconds)
        JH // Appears to be successful connection response?
    }
}
