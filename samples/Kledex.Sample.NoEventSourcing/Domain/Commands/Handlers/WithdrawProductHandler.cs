using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Commands;
using Kledex.Domain;
using Kledex.Events;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Sample.NoEventSourcing.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands.Handlers
{
    public class WithdrawProductHandler : ICommandHandlerAsync<WithdrawProduct>
    {
        private readonly SampleDbContext _dbContext;

        public WithdrawProductHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> HandleAsync(WithdrawProduct command)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {command.AggregateRootId}");
            }

            product.Withdraw();

            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>
                {
                    new ProductWithdrew
                    {
                        AggregateRootId = product.Id
                    }
                }
            };
        }
    }
}
