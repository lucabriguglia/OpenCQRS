using Kledex.Commands;
using Kledex.Sample.CommandSequence.Commands;
using System;
using System.Threading.Tasks;

namespace Kledex.Sample.CommandSequence.Handlers
{
    public class FirstCommandHandler : ISequenceCommandHandlerAsync<FirstCommand>
    {
        public Task<CommandResponse> HandleAsync(FirstCommand command, CommandResponse previousStepResponse)
        {
            Console.WriteLine("Message from first command handler");

            return Task.FromResult(new CommandResponse
            { 
                Result = "First result"
            });
        }
    }
}
