using MongoDB.Driver;
using Pacote.Core.Domain.Util.DocumentDB;
using PersisteDocDb.Lambda.Domain.Entities;

namespace PersisteDocDb.Lambda.Infrastructure.Repositories
{
    public class PosicaoDocumentRepository : IDocumentRepository<PosicaoDocument>
    {
        private readonly IDocumentCollection<PosicaoDocument> _collection;
        public PosicaoDocumentRepository(IDocumentCollection<PosicaoDocument> collection)
        {
            _collection = collection;
        }
        public long PersistirDocumentReplaceOne(PosicaoDocument document)
        {
            var filter = Builders<PosicaoDocument>.Filter.Eq("_id", document.Id);
            var result = _collection.ReplaceOne(filter, document, new ReplaceOptions { IsUpsert = true });
            return result.ModifiedCount;
        }

        //public async Task InserirPosicaoAsync(PosicaoDocument posicaoDocument)
        //{
        //    await _collection.CreateDocumentAsync(posicaoDocument);
        //}

        //public Task InserirPosicao(PosicaoDocument posicaoDocument)
        //{
        //    _collection.CreateDocumentAsync(posicaoDocument).ConfigureAwait(true);
        //    return Task.CompletedTask;
        //}
    }
}
