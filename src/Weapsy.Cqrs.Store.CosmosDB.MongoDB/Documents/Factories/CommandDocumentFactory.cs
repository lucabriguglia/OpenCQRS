using System;
using Newtonsoft.Json;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Store.CosmosDB.MongoDB.Documents.Factories
{
    public class CommandDocumentFactory : ICommandDocumentFactory
    {
        public CommandDocument CreateCommand(IDomainCommand command)
        {
            return new CommandDocument
            {
                Id = Guid.NewGuid().ToString(),
                AggregateId = command.AggregateRootId.ToString(),
                Type = command.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(command),
                TimeStamp = command.TimeStamp,
                UserId = command.UserId,
                Source = command.Source
            };
        }
    }
}