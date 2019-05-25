using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Store.Cosmos.Sql.Documents;
using Kledex.Store.Cosmos.Sql.Documents.Factories;
using Kledex.Store.Cosmos.Sql.Repositories;

namespace Kledex.Store.Cosmos.Sql
{
    /// <inheritdoc />
    internal class AggregateStore : IAggregateStore
    {
        private readonly IDocumentRepository<AggregateDocument> _aggregateRepository;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;

        public AggregateStore(IDocumentRepository<AggregateDocument> aggregateRepository, IAggregateDocumentFactory aggregateDocumentFactory)
        {
            _aggregateRepository = aggregateRepository;
            _aggregateDocumentFactory = aggregateDocumentFactory;
        }

        /// <inheritdoc />
        public async Task SaveAggregateAsync<TAggregate>(Guid id) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = await _aggregateRepository.GetDocumentAsync(id.ToString());
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(id);
                await _aggregateRepository.CreateDocumentAsync(newAggregateDocument);
            }
        }

        /// <inheritdoc />
        public void SaveAggregate<TAggregate>(Guid id) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = _aggregateRepository.GetDocumentAsync(id.ToString()).GetAwaiter().GetResult();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(id);
                _aggregateRepository.CreateDocumentAsync(newAggregateDocument).GetAwaiter().GetResult();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AggregateStoreModel>> GetAggregatesAsync()
        {
            var aggregateDocuments = await _aggregateRepository.GetDocumentsAsync(_ => true);
            return aggregateDocuments.Select(aggregateDocument => new AggregateStoreModel
            {
                Id = aggregateDocument.Id,
                Type = aggregateDocument.Type
            }).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<AggregateStoreModel> GetAggregates()
        {
            var aggregateDocuments = _aggregateRepository.GetDocumentsAsync(_ => true).GetAwaiter().GetResult();
            return aggregateDocuments.Select(aggregateDocument => new AggregateStoreModel
            {
                Id = aggregateDocument.Id,
                Type = aggregateDocument.Type
            }).ToList();
        }
    }
}
