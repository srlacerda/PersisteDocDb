using MongoDB.Driver;
using Pacote.Core.Domain.Util.DocumentDB;
using PersisteDocDb.Lambda.Domain.Entities;

namespace PersisteDocDb.Lambda.Infrastructure.Repositories
{
    public class OperacaoDocumentRepository : IDocumentRepository<OperacaoDocument>
    {
        private readonly IDocumentCollection<OperacaoDocument> _collection;
        public OperacaoDocumentRepository(IDocumentCollection<OperacaoDocument> collection)
        {
            _collection = collection;
        }
        public long PersistirDocumentReplaceOne(OperacaoDocument document)
        {
            var filter = Builders<OperacaoDocument>.Filter.Eq("_id", document.Id);
            var result = _collection.ReplaceOne(filter, document, new ReplaceOptions { IsUpsert = true });
            return result.ModifiedCount;
        }
    }
}
