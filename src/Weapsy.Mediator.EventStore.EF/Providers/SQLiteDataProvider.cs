//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Weapsy.Mediator.EventStore.EF.Configuration;

//// ReSharper disable InconsistentNaming

//namespace Weapsy.Mediator.EventStore.EF.Providers
//{
//    public class SQLiteDataProvider : IDataProvider
//    {
//        public DataProvider Provider { get; } = DataProvider.SQLite;

//        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
//        {
//            services.AddDbContext<MediatorDbContext>(options =>
//                options.UseSqlite(connectionString));

//            return services;
//        }

//        public MediatorDbContext CreateDbContext(string connectionString)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<MediatorDbContext>();
//            optionsBuilder.UseSqlite(connectionString);

//            return new MediatorDbContext(optionsBuilder.Options);
//        }
//    }
//}