using Kledex.Commands;
using Kledex.Domain;
using Kledex.Transactions;
using System;
using System.Threading.Tasks;

namespace Kledex.Store.EF
{
    public class TransactionService : ITransactionService
    {
        private readonly IDomainDbContextFactory _dbContextFactory;
        private readonly ICommandSender _commandSender;

        public TransactionService(IDomainDbContextFactory dbContextFactory,
            ICommandSender commandSender)
        {
            _dbContextFactory = dbContextFactory;
            _commandSender = commandSender;
        }

        public async Task ProcessAsync(ICommand command)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        command.Properties.Add("DbContextTransaction", transaction);

                        await _commandSender.SendAsync(command);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex;
                    }
                }
            }
        }
    }
}
