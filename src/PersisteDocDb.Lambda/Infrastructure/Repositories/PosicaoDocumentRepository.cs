using Pacote.Core.Domain.Util.DocumentDB;
using PersisteDocDb.Lambda.Domain.Entities;
using System.Threading.Tasks;

namespace PersisteDocDb.Lambda.Infrastructure.Repositories
{
    public class PosicaoDocumentRepository : IPosicaoDocumentRepository
    {
        private readonly IDocumentCollection<PosicaoDocument> _collection;
        public PosicaoDocumentRepository(IDocumentCollection<PosicaoDocument> collection)
        {
            _collection = collection;
        }
        //public async Task InserirPosicaoAsync(PosicaoDocument posicaoDocument)
        public async Task InserirPosicaoAsync(PosicaoDocument posicaoDocument)
        {
            //await _collection.CreateDocumentAsync(posicaoDocument);
            await _collection.CreateDocumentAsync(posicaoDocument);
            //await Task.CompletedTask;
        }
    }
}
