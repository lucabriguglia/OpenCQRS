//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Weapsy.Mediator.EventStore.EF.Configuration;

//// ReSharper disable InconsistentNaming

//namespace Weapsy.Mediator.EventStore.EF.Providers
//{
//    public class MySQLDataProvider : IDataProvider
//    {
//        public DataProvider Provider { get; } = DataProvider.MySQL;

//        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
//        {
//            services.AddDbContext<MediatorDbContext>(options => 
//                options.UseMySQL(connectionString));

//            return services;
//        }

//        public MediatorDbContext CreateDbContext(string connectionString)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<MediatorDbContext>();
//            optionsBuilder.UseMySQL(connectionString);

//            return new MediatorDbContext(optionsBuilder.Options);
//        }
//    }
//}