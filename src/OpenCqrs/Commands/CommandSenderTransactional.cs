using System.Threading.Tasks;
using System.Transactions;
using OpenCqrs.Abstractions.Commands;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Dependencies;
using OpenCqrs.Domain;
using OpenCqrs.Events;

namespace OpenCqrs.Commands
{
    //public class CommandSenderTransactional : CommandSender
    //{
    //    public CommandSenderTransactional(
    //        IHandlerResolver handlerResolver,
    //        IEventPublisher eventPublisher,
    //        IEventFactory eventFactory,
    //        IEventStore eventStore,
    //        ICommandStore commandStore)
    //        : base(handlerResolver, eventPublisher, eventFactory, eventStore, commandStore)
    //    {
    //    }

    //    public new async Task SendAndPublishAsync<TCommand>(TCommand command) where TCommand : ICommand
    //    {
    //        using (var trx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    //        {
    //            await base.SendAndPublishAsync<TCommand>(command);
    //            trx.Complete();
    //        }
    //    }

    //    public new async Task SendAndPublishAsync<TCommand, TAggregate>(TCommand command)
    //        where TCommand : IDomainCommand
    //        where TAggregate : IAggregateRoot
    //    {
    //        using (var trx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    //        {
    //            await base.SendAndPublishAsync<TCommand, TAggregate>(command);
    //            trx.Complete();
    //        }
    //    }

    //    public new void SendAndPublish<TCommand>(TCommand command) where TCommand : ICommand
    //    {
    //        using (var trx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    //        {
    //            base.SendAndPublish<TCommand>(command);
    //            trx.Complete();
    //        }
    //    }

    //    public new void SendAndPublish<TCommand, TAggregate>(TCommand command)
    //        where TCommand : IDomainCommand
    //        where TAggregate : IAggregateRoot
    //    {
    //        using (var trx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    //        {
    //            base.SendAndPublish<TCommand, TAggregate>(command);
    //            trx.Complete();
    //        }
    //    }
    //}
}