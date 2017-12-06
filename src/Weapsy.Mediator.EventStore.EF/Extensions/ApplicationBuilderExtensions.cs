using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Mediator.EventStore.EF.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureMediatorDbCreated(this IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetService<MediatorDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}