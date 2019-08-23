using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Kledex.Store.Cosmos.Sql.Repositories
{
    public interface IDocumentRepository<TDocument> where TDocument : class
    {
        Task<Document> CreateDocumentAsync(TDocument document);
        Task<TDocument> GetDocumentAsync(string documentId);
        Task<IList<TDocument>> GetDocumentsAsync(Expression<Func<TDocument, bool>> predicate);
        Task<int> GetCountAsync(Expression<Func<TDocument, bool>> predicate);
    }
}