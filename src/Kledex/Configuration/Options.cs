namespace Kledex.Configuration
{
    public class Options
    {
        /// <summary>
        /// The value indicating whether events are published automatically. Default value is true.
        /// </summary>
        public bool PublishEvents { get; set; } = true;

        /// <summary>
        /// The value indicating whether commands are saved alongside events. Default value is true.
        /// </summary>
        public bool SaveCommandData { get; set; } = true;

        /// <summary>
        /// The value indicating whether to validate commands. Default value is false.
        /// </summary>
        public bool ValidateCommands { get; set; } = false;

        /// <summary>
        /// The value indicating the cache time (in seconds). Default value is 60.
        /// </summary>
        public int CacheTime { get; set; } = 60;
    }
}
