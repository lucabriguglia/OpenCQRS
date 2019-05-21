namespace OpenCqrs.Store.EF
{
    public static class Constants
    {
        public const string DomainDbConfigurationSection = "DomainDbConfiguration";
        public static readonly string DomainDbConfigurationConnectionString = $"{DomainDbConfigurationSection}:ConnectionString";
    }
}
