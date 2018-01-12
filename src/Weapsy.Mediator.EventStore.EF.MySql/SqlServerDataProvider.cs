using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable InconsistentNaming

namespace Weapsy.Mediator.EventStore.EF.MySql
{
    public class SqlServerDataProvider : IDataProvider
    {
        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MediatorDbContext>(options =>
                options.UseMySQL(connectionString));

            return services;
        }

        public MediatorDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MediatorDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            return new MediatorDbContext(optionsBuilder.Options);
        }
    }
}