using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Weapsy.Mediator.EventStore.CosmosDB.Sql
{
    internal interface IDocumentDbRepository<TDocument> where TDocument : class
    {
        Task<Document> CreateDocumentAsync(string collectionId, TDocument document);
        Task<TDocument> GetDocumentAsync(string collectionId, string id);
        Task<IList<TDocument>> GetDocumentsAsync(string collectionId, Expression<Func<TDocument, bool>> predicate);
        Task<int> GetCountAsync(string collectionId, Expression<Func<TDocument, bool>> predicate);
    }
}