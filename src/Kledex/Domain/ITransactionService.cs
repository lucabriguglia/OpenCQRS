using System;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface ITransactionService
    {
        Task ProcessAsync<TAggregate>(IDomainCommand<TAggregate> command)
            where TAggregate : IAggregateRoot;

        void Process<TAggregate>(IDomainCommand<TAggregate> command)
            where TAggregate : IAggregateRoot;
    }

    public class DefaultTransactionService : ITransactionService
    {
        public void Process<TAggregate>(IDomainCommand<TAggregate> command) 
            where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException(Consts.TransactionServiceRequiredMessage);
        }

        public Task ProcessAsync<TAggregate>(IDomainCommand<TAggregate> command) 
            where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException(Consts.TransactionServiceRequiredMessage);
        }
    }
}
