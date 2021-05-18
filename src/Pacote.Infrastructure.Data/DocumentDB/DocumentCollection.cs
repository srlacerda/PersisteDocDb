using MongoDB.Driver;
using Pacote.Core.Domain.Util.DocumentDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pacote.Infrastructure.Data.DocumentDB
{
    public class DocumentCollection<T> : IDocumentCollection<T> where T : IDocument
    {

        private readonly IMongoCollection<T> _collection;

        public DocumentCollection(IMongoClient client, string databaseId, string collectionId)
        {
            var db = client.GetDatabase(databaseId);
            _collection = db.GetCollection<T>(collectionId);
        }
        public Task CreateDocumentAsync(T document)
        {
            return _collection.InsertOneAsync(document);
        }

        public Task DeleteDocumentAsync(string documentId)
        {
            return _collection.DeleteOneAsync(d => d.Id == documentId);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter = null, int page = 0, int size = 50)
        {
            var options = new FindOptions<T, T>();
            options.Skip = page ;
            options.Limit = size;

            var result = await _collection.FindAsync(filter, options);

            return await result.ToListAsync();
        }

        public async Task<List<T>> FindAsync(FilterDefinition<T> filter = null, int page = 0, int size = 50, string sort = "desc", string orderBy = "StoreNumber")
        {
            var options = new FindOptions<T, T>();
            options.Skip = page * size;
            options.Limit = size;

            if (sort == "desc")
            {
                options.Sort = Builders<T>.Sort.Descending(orderBy);
            }
            else
            {
                options.Sort = Builders<T>.Sort.Ascending(orderBy);
            }

            var result = await _collection.FindAsync(filter, options);

            return await result.ToListAsync();
        }

        public ReplaceOneResult ReplaceOne(FilterDefinition<T> filter, T document, ReplaceOptions options = null)
        {
            return _collection.ReplaceOne(filter, document, options);
        }
    }
}
