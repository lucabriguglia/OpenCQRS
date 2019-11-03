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
        /// The value indicating whether to use transactions. Default value is false.
        /// </summary>
        public bool UseTransactions { get; set; } = false;
    }
}
