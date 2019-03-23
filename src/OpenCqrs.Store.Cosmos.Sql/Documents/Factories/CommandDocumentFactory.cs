using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenCqrs.Domain;
using Options = OpenCqrs.Configuration.Options;

namespace OpenCqrs.Store.Cosmos.Sql.Documents.Factories
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