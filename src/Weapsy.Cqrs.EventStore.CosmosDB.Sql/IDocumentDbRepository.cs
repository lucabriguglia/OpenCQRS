using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Weapsy.Cqrs.EventStore.CosmosDB.Sql
{
    internal interface IDocumentDbRepository<TDocument> where TDocument : class
    {
        Task<Document> CreateDocumentAsync(TDocument document);
        Task<TDocument> GetDocumentAsync(string id);
        Task<IList<TDocument>> GetDocumentsAsync(Expression<Func<TDocument, bool>> predicate);
        Task<int> GetCountAsync(Expression<Func<TDocument, bool>> predicate);
    }
}