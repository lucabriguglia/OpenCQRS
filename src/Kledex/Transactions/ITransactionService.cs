using System;
using System.Threading.Tasks;

namespace Kledex.Transactions
{
    public interface ITransactionService
    {
        Task ProcessAsync(Func<Task> execute);
    }
}
