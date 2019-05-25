using Kledex.Domain;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Options = Kledex.Configuration.Options;

namespace Kledex.Store.Cosmos.Mongo.Documents.Factories
{
    public class CommandDocumentFactory : ICommandDocumentFactory
    {
        private readonly Options _options;

        private bool SaveCommandData(IDomainCommand command) => command.SaveCommandData ?? _options.SaveCommandData;

        public CommandDocumentFactory(IOptions<Options> options)
        {
            _options = options.Value;
        }

        public CommandDocument CreateCommand(IDomainCommand command)
        {
            return new CommandDocument
            {
                Id = command.Id.ToString(),
                AggregateId = command.AggregateRootId.ToString(),
                Type = command.GetType().AssemblyQualifiedName,
                Data = SaveCommandData(command) ? JsonConvert.SerializeObject(command) : null,
                TimeStamp = command.TimeStamp,
                UserId = command.UserId,
                Source = command.Source
            };
        }
    }
}
