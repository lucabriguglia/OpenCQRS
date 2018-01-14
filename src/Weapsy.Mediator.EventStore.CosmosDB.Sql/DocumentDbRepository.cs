using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using Weapsy.Mediator.EventStore.CosmosDB.Sql.Configuration;

namespace Weapsy.Mediator.EventStore.CosmosDB.Sql
{
    internal class DocumentDbRepository<TDocument> : IDocumentDbRepository<TDocument> where TDocument : class
    {
        private readonly IDocumentClient _documentClient;
        private readonly string _databaseId;

        public DocumentDbRepository(IDocumentClient documentClient, IOptions<CosmosDBSettings> settings)
        {
            _documentClient = documentClient;
            _databaseId = settings.Value.DatabaseId;
        }

        public async Task<Document> CreateDocumentAsync(string collectionId, TDocument document)
        {
            return await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, collectionId), document);
        }

        public async Task<TDocument> GetDocumentAsync(string collectionId, string id)
        {
            try
            {
                Document document = await _documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, collectionId, id));
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

        public async Task<IList<TDocument>> GetDocumentsAsync(string collectionId, Expression<Func<TDocument, bool>> predicate)
        {
            var query = _documentClient.CreateDocumentQuery<TDocument>(
                    UriFactory.CreateDocumentCollectionUri(_databaseId, collectionId),
                    new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<TDocument>();

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<TDocument>());
            }

            return results;
        }

        public async Task<int> GetCountAsync(string collectionId, Expression<Func<TDocument, bool>> predicate)
        {
            return await _documentClient.CreateDocumentQuery<TDocument>(
                    UriFactory.CreateDocumentCollectionUri(_databaseId, collectionId))
                .Where(predicate).CountAsync();
        }
    }
}