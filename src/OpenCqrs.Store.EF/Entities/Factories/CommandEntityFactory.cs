using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCqrs.Domain;
using Options = OpenCqrs.Configuration.Options;

namespace OpenCqrs.Store.EF.Entities.Factories
{
    public class CommandEntityFactory : ICommandEntityFactory
    {
        private readonly Options _options;

        private bool SaveCommandData(IDomainCommand command) => command.SaveCommandData ?? _options.SaveCommandData;

        public CommandEntityFactory(IOptions<Options> options)
        {
            _options = options.Value;
        }

        public CommandEntity CreateCommand(IDomainCommand command)
        {
            return new CommandEntity
            {
                Id = command.Id,
                AggregateId = command.AggregateRootId,
                Type = command.GetType().AssemblyQualifiedName,
                Data = SaveCommandData(command) ? JsonConvert.SerializeObject(command) : null,
                TimeStamp = command.TimeStamp,
                UserId = command.UserId,
                Source = command.Source
            };
        }
    }
}