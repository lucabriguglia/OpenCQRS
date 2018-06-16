using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OpenCqrs.Store.EF.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureDomainDbCreated(this IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetService<DomainDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}