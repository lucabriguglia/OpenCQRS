using Kledex.Dependencies;
using Kledex.Domain;
using System;
using System.Threading.Tasks;

namespace Kledex.Store.EF
{
    public class TransactionService : ITransactionService
    {
        private readonly IDomainDbContextFactory _dbContextFactory;
        private readonly IResolver _resolver;

        public TransactionService(IDomainDbContextFactory dbContextFactory, IResolver resolver)
        {
            _dbContextFactory = dbContextFactory;
            _resolver = resolver;
        }

        public void Process<TAggregate>(IDomainCommand<TAggregate> command) where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException();
        }

        public async Task ProcessAsync<TAggregate>(IDomainCommand<TAggregate> command) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    command.Properties.Add(Consts.DbContextTransactionKey, transaction);
                    var serviceType = typeof(IDomainCommandSender).MakeGenericType();
                    var service = _resolver.Resolve(serviceType);
                    var sendMethod = service.GetType().GetMethod("SendAsync");
                    await (Task)sendMethod.Invoke(service, new object[] { command });
                }
            }
        }
    }
}
