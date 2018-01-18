using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Cqrs.EventStore.EF.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureEventStoreDbCreated(this IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetService<EventStoreDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}