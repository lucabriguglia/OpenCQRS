using System;
using System.Threading.Tasks;
using System.Transactions;

namespace OpenCqrs.Helpers
{
    public static class TransactionHelper {
        public static async Task ExecuteInTransactionIf(bool conditional, Func<Task> action)
        {
            if (conditional)
            {
                using (var trx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await action();
                    trx.Complete();
                }
            }
            else
            {
                await action();
            }
        }

        public static void ExecuteInTransactionIf(bool conditional, Action action)
        {
            if (conditional)
            {
                using (var trx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    action();
                    trx.Complete();
                }
            }
            else
            {
                action();
            }
        }
    }
}