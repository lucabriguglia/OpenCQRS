using Newtonsoft.Json;
using OpenCqrs.Configuration;
using OpenCqrs.Domain;

namespace OpenCqrs.Store.EF.Entities.Factories
{
    public class CommandEntityFactory : ICommandEntityFactory
    {
        private readonly Options _options;

        private bool SaveCommandData(IDomainCommand command) => command.SaveCommandData ?? _options.SaveCommandData;

        public CommandEntityFactory(Options options)
        {
            _options = options;
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