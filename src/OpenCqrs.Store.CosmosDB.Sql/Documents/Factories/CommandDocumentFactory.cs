using System;
using Newtonsoft.Json;
using OpenCqrs.Domain;

namespace OpenCqrs.Store.CosmosDB.Sql.Documents.Factories
{
    public class CommandDocumentFactory : ICommandDocumentFactory
    {
        public CommandDocument CreateCommand(IDomainCommand command)
        {
            return new CommandDocument
            {
                Id = command.Id,
                AggregateId = command.AggregateRootId,
                Type = command.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(command),
                TimeStamp = command.TimeStamp ?? DateTime.UtcNow,
                UserId = command.UserId,
                Source = command.Source
            };
        }
    }
}