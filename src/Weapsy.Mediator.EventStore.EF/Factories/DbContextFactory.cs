using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Mediator.EventStore.EF.Factories
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public MediatorDbContext CreateDbContext()
        {
            var options = _serviceProvider.GetService<DbContextOptions<MediatorDbContext>>();
            return new MediatorDbContext(options);
        }
    }
}