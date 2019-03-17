using System;
using System.Threading.Tasks;
using OpenCqrs.Domain;
using OpenCqrs.Store.Cosmos.Sql.Documents;
using OpenCqrs.Store.Cosmos.Sql.Documents.Factories;
using OpenCqrs.Store.Cosmos.Sql.Repositories;

namespace OpenCqrs.Store.Cosmos.Sql
{
    internal class AggregateStore : IAggregateStore
    {
        private readonly IDocumentRepository<AggregateDocument> _aggregateRepository;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;

        public AggregateStore(IDocumentRepository<AggregateDocument> aggregateRepository, IAggregateDocumentFactory aggregateDocumentFactory)
        {
            _aggregateRepository = aggregateRepository;
            _aggregateDocumentFactory = aggregateDocumentFactory;
        }

        public async Task SaveAggregateAsync<TAggregate>(Guid id) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = await _aggregateRepository.GetDocumentAsync(id.ToString());
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(id);
                await _aggregateRepository.CreateDocumentAsync(newAggregateDocument);
            }
        }

        public void SaveAggregate<TAggregate>(Guid id) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = _aggregateRepository.GetDocumentAsync(id.ToString()).GetAwaiter().GetResult();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(id);
                _aggregateRepository.CreateDocumentAsync(newAggregateDocument).GetAwaiter().GetResult();
            }
        }
    }
}
