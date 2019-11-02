using Kledex.Commands;
using Kledex.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Sample.EventSourcing.Services
{
    public class SendEmailHandler : ICommandHandlerAsync<SendEmail>
    {
        public Task<CommandResponse> HandleAsync(SendEmail command)
        {
            return Task.FromResult(new CommandResponse
            { 
                Events = new List<IEvent>()
                {
                    new EmailSent()
                }
            });
        }
    }
}
