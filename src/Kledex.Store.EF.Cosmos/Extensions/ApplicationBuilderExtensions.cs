using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.Cosmos.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IKledexAppBuilder EnsureCosmosDbCreated(this IKledexAppBuilder builder)
        {
            var documentClient = builder.App.ApplicationServices.GetService<IDomainDbContextFactory>();

            using (var dbContext = documentClient.CreateDbContext())
            {
                dbContext.Database.EnsureCreated();
            }

            return builder;
        }
    }
}