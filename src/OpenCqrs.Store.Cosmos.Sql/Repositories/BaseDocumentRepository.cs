using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using OpenCqrs.Store.Cosmos.Sql.Configuration;

namespace OpenCqrs.Store.Cosmos.Sql.Repositories
{
    internal abstract class BaseDocumentRepository<TDocument> : IDocumentRepository<TDocument> where TDocument : class
    {
        private readonly IDocumentClient _documentClient;
        private readonly string _databaseId;
        private readonly string _collectionId;

        protected BaseDocumentRepository(string collectionId, IDocumentClient documentClient, IOptions<DomainDbConfiguration> settings)
        {
            _documentClient = documentClient;
            _databaseId = settings.Value.DatabaseId;
            _collectionId = collectionId;
        }

        public async Task<Document> CreateDocumentAsync(TDocument document)
        {
            return await _documentClient.CreateDocumentAsync(GetUri(), document);
        }

        public async Task<TDocument> GetDocumentAsync(string documentId)
        {
            try
            {
                Document document = await _documentClient.ReadDocumentAsync(GetUri(documentId));
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
                .CreateDocumentQuery<TDocument>(GetUri(), new FeedOptions { MaxItemCount = -1 })
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
                .CreateDocumentQuery<TDocument>(GetUri())
                .Where(predicate)
                .CountAsync();
        }

        private Uri GetUri(string documentId = "")
        {
            return !string.IsNullOrEmpty(documentId) 
                ? UriFactory.CreateDocumentUri(_databaseId, _collectionId, documentId) 
                : UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
        }
    }
}