using Kledex.Transactions;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Kledex.Store.EF
{
    public class TransactionService : ITransactionService
    {
        public async Task ProcessAsync(Func<Task> execute)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await execute();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    var ex1 = ex;
                }
            }
        }
    }
}
