namespace OpenCqrs.Configuration
{
    public class Options
    {
        /// <summary>
        /// The value indicating whether commands are saved along side events. Default value is true.
        /// </summary>
        public bool SaveCommands { get; set; } = true;
    }
}
