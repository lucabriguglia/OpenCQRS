using Kledex.Commands;
using System.Threading.Tasks;

namespace Kledex.Transactions
{
    public interface ITransactionService
    {
        Task ProcessAsync(ICommand command);
    }
}
