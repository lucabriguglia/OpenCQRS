using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Configuration;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Documents;

namespace Weapsy.Cqrs.Store.CosmosDB.Sql
{
    internal class DocumentDbRepository<TDocument> : IDocumentDbRepository<TDocument> where TDocument : class
    {
        private readonly IDocumentClient _documentClient;
        private readonly IOptions<StoreConfiguration> _settings;

        public DocumentDbRepository(IDocumentClient documentClient, IOptions<StoreConfiguration> settings)
        {
            _documentClient = documentClient;
            _settings = settings;
        }

        public async Task<Document> CreateDocumentAsync(TDocument document)
        {
            return await _documentClient.CreateDocumentAsync(GetUri(), document);
        }

        public async Task<TDocument> GetDocumentAsync(string id)
        {
            try
            {
                Document document = await _documentClient.ReadDocumentAsync(GetUri(id));
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

        public async Task<int> GetCountAsync(Expression<Func<TDocument, bool>> predicate)
        {
            return await _documentClient
                .CreateDocumentQuery<TDocument>(GetUri())
                .Where(predicate)
                .CountAsync();
        }

        private Uri GetUri(string documentId = "")
        {
            var databaseId = _settings.Value.DatabaseId;

            var collectionId = typeof(TDocument) == typeof(AggregateDocument)
                ? _settings.Value.AggregateCollectionId
                : typeof(TDocument) == typeof(CommandDocument)
                    ? _settings.Value.CommandCollectionId
                    : _settings.Value.EventCollectionId;

            return string.IsNullOrEmpty(documentId)
                ? UriFactory.CreateDocumentCollectionUri(databaseId, collectionId)
                : UriFactory.CreateDocumentUri(databaseId, collectionId, documentId);
        }
    }
}