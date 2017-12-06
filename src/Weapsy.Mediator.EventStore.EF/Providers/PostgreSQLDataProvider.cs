//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Weapsy.Mediator.EventStore.EF.Configuration;

//// ReSharper disable InconsistentNaming

//namespace Weapsy.Mediator.EventStore.EF.Providers
//{
//    public class PostgreSQLDataProvider : IDataProvider
//    {
//        public DataProvider Provider { get; } = DataProvider.PostgreSQL;

//        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
//        {
//            services.AddDbContext<MediatorDbContext>(options =>
//                options.UseNpgsql(connectionString));

//            return services;
//        }

//        public MediatorDbContext CreateDbContext(string connectionString)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<MediatorDbContext>();
//            optionsBuilder.UseNpgsql(connectionString);

//            return new MediatorDbContext(optionsBuilder.Options);
//        }
//    }
//}