using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kledex.Store.Cosmos.Sql.Repositories
{
    public abstract class BaseDocumentRepository<TDocument> : IDocumentRepository<TDocument> where TDocument : class
    {
        private readonly IDocumentClient _documentClient;
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly PartitionKey _partitionKey;
        private readonly int? _offerThroughput;
        private readonly ConsistencyLevel? _consistencyLevel;

        protected BaseDocumentRepository(string collectionId, IDocumentClient documentClient, IOptions<DomainDbOptions> settings)
        {
            _documentClient = documentClient;
            _databaseId = settings.Value.DatabaseId;
            _collectionId = collectionId;
            _partitionKey = settings.Value.PartitionKey;
            _offerThroughput = settings.Value.OfferThroughput;
            _consistencyLevel = settings.Value.ConsistencyLevel;
        }

        public async Task<Document> CreateDocumentAsync(TDocument document)
        {
            return await _documentClient.CreateDocumentAsync(GetUri(), document, GetRequestOptions());
        }

        public async Task<TDocument> GetDocumentAsync(string documentId)
        {
            try
            {
                Document document = await _documentClient.ReadDocumentAsync(GetUri(documentId), GetRequestOptions());
                return (TDocument)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public async Task<IList<TDocument>> GetDocumentsAsync(Expression<Func<TDocument, bool>> predicate)
        {
            var query = _documentClient
                .CreateDocumentQuery<TDocument>(GetUri(), GetFeedOptions())
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<TDocument>();

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<TDocument>());
            }

            return results;
        }

        public Task<int> GetCountAsync(Expression<Func<TDocument, bool>> predicate)
        {
            return _documentClient
                .CreateDocumentQuery<TDocument>(GetUri(), GetFeedOptions())
                .Where(predicate)
                .CountAsync();
        }

        private Uri GetUri(string documentId = "")
        {
            return !string.IsNullOrEmpty(documentId) 
                ? UriFactory.CreateDocumentUri(_databaseId, _collectionId, documentId) 
                : UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
        }

        private RequestOptions GetRequestOptions()
        {
            return new RequestOptions
            {
                PartitionKey = _partitionKey,
                OfferThroughput = _offerThroughput,
                ConsistencyLevel = _consistencyLevel
            };
        }

        private FeedOptions GetFeedOptions()
        {
            return new FeedOptions
            {
                PartitionKey = _partitionKey,
                ConsistencyLevel = _consistencyLevel,
                MaxItemCount = -1
            };
        }
    }
}