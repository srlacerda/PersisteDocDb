using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pacote.Core.Domain.Util.DocumentDB
{
    public interface IDocumentCollection<T> where T : IDocument
    {
        Task CreateDocumentAsync(T document);
        Task DeleteDocumentAsync(string documentId);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> filter = null, int page = 0, int size = 50);
        Task<List<T>> FindAsync(FilterDefinition<T> filter = null, int page = 0, int size = 50, string sort = "desc", string orderBy = "StoreNumber");
        ReplaceOneResult ReplaceOne(FilterDefinition<T> filter, T document, ReplaceOptions options = null);
    }
}
