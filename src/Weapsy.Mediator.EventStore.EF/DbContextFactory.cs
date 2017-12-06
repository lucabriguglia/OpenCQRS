using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.EventStore.EF.Configuration;

namespace Weapsy.Mediator.EventStore.EF
{
    public class DbContextFactory : IDbContextFactory
    {
        private MediatorData DataConfiguration { get; }
        private ConnectionStrings ConnectionStrings { get; }
        private readonly IResolver _resolver;

        public DbContextFactory(IOptions<MediatorData> dataOptions,
            IOptions<ConnectionStrings> connectionStringsOptions,
            IResolver resolver)
        {
            DataConfiguration = dataOptions.Value;
            ConnectionStrings = connectionStringsOptions.Value;
            _resolver = resolver;
        }

        public MediatorDbContext CreateDbContext()
        {
            var dataProvider = _resolver.ResolveAll<IDataProvider>().SingleOrDefault(x => x.Provider == DataConfiguration.EFProvider);

            if (dataProvider == null)
                throw new Exception("The Data Provider entry in appsettings.json is empty or the one specified has not been found.");

            return dataProvider.CreateDbContext(ConnectionStrings.MediatorConnection);
        }
    }
}
