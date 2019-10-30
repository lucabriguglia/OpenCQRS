using System;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface ITransactionService
    {
        Task ProcessAsync();
    }

    public class DefaultTransactionService : ITransactionService
    {
        public Task ProcessAsync()
        {
            throw new NotImplementedException(Consts.TransactionServiceRequiredMessage);
        }
    }
}
