using System.Transactions;
using RabbitMQ.Client;

namespace OpenCqrs.Bus.Rabbitmq.Shared
{
    sealed class RabbitmqResourceManager : IEnlistmentNotification
    {
        private readonly IModel _channel;

        private RabbitmqResourceManager(IModel channel, Transaction transaction)
        {
            _channel = channel;
            _channel.TxSelect();
            transaction.EnlistVolatile(this, EnlistmentOptions.None);
        }

        public static RabbitmqResourceManager EnlistToTransaction(IModel channel, Transaction transaction) =>
            new RabbitmqResourceManager(channel, transaction);

        public RabbitmqResourceManager(IModel channel)
        {
            _channel = channel;
            _channel.TxSelect();
            if (Transaction.Current != null)
                Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
        }

        public void Commit(Enlistment enlistment)
        {
            _channel.TxCommit();
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            Rollback(enlistment);
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        public void Rollback(Enlistment enlistment)
        {
            _channel.TxRollback();
            enlistment.Done();
        }
    }
}