using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Extensions;

namespace OpenCqrs.Store.EF.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IKledexAppBuilder EnsureDomainDbCreated(this IKledexAppBuilder builder)
        {
            using (var serviceScope = builder.App.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DomainDbContext>();
                dbContext.Database.Migrate();
            }

            return builder;
        }
    }
}