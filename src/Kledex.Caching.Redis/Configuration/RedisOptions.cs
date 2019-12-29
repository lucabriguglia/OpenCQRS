namespace Kledex.Caching.Redis.Configuration
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public int Db { get; set; } = -1;
        public object AsyncState { get; set; } = null;
    }
}
