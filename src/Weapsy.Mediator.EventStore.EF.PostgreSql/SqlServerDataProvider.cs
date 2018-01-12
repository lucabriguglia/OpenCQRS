using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable InconsistentNaming

namespace Weapsy.Mediator.EventStore.EF.PostgreSql
{
    public class SqlServerDataProvider : IDataProvider
    {
        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MediatorDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }

        public MediatorDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MediatorDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new MediatorDbContext(optionsBuilder.Options);
        }
    }
}