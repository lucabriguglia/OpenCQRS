using Kledex.Domain;
using System;
using System.Threading.Tasks;

namespace Kledex.Store.EF
{
    public class TransactionService : ITransactionService
    {
        private readonly IDomainDbContextFactory _dbContextFactory;
        private readonly IDomainCommandSender _domainCommandSender;

        public TransactionService(IDomainDbContextFactory dbContextFactory, IDomainCommandSender domainCommandSender)
        {
            _dbContextFactory = dbContextFactory;
            _domainCommandSender = domainCommandSender;
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
                    await _domainCommandSender.SendAsync(command);
                }
            }
        }
    }
}
