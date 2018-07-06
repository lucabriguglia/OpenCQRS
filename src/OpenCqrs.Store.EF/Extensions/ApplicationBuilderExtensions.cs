using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Extensions;

namespace OpenCqrs.Store.EF.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IOpenCqrsAppBuilder EnsureDomainDbCreated(this IOpenCqrsAppBuilder builder)
        {
            var dbContext = builder.App.ApplicationServices.GetRequiredService<DomainDbContext>();

            dbContext.Database.Migrate();

            return builder;
        }
    }
}