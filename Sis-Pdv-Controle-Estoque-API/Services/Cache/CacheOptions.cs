namespace Sis_Pdv_Controle_Estoque_API.Services.Cache
{
    public class CacheOptions
    {
        public const string SectionName = "Cache";

        public int DefaultExpirationMinutes { get; set; } = 30;
        public int SlidingExpirationMinutes { get; set; } = 10;
        public bool EnableCaching { get; set; } = true;
        public int MaxCacheSize { get; set; } = 1000;
    }
}