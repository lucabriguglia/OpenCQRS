using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mediator.EventStore.EF.Configuration;

// ReSharper disable InconsistentNaming

namespace Weapsy.Mediator.EventStore.EF.Providers
{
    public class MSSQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MSSQL;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MediatorDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public MediatorDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MediatorDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MediatorDbContext(optionsBuilder.Options);
        }
    }
}