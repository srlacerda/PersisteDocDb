using Newtonsoft.Json;
using Pacote.Core.Domain.Model.Contratos;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Domain.Entities;

namespace PersisteDocDb.Lambda.Infrastructure.Factory
{
    public class PosicaoFactory : IDocumentFactory<PosicaoDocument>
    {
        public PublicarDocumentPersistidoCommand CreatePublicarDocumentPersistidoCommand(PosicaoDocument document)
        {
            var documentPersistido = GetDocumentPersistidoByPosicao(document);
            var publicarDocumentPersistidoCommand = new PublicarDocumentPersistidoCommand
            {
                Message = JsonConvert.SerializeObject(documentPersistido)
            };
            return publicarDocumentPersistidoCommand;
        }
        public PosicaoDocument DeserializeDocument(string message)
        {
            return JsonConvert.DeserializeObject<PosicaoDocument>(message);
        }
        private DocumentPersistido GetDocumentPersistidoByPosicao(PosicaoDocument document)
        {
            return new DocumentPersistido
            {
                DocumentCollection = "posicao",
                Id = document.Id
            };
        }
    }
}
