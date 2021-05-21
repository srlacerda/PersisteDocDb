using Newtonsoft.Json;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Domain.Entities;

namespace PersisteDocDb.Lambda.Infrastructure.Factory
{
    public class OperacaoFactory : IDocumentFactory<OperacaoDocument>
    {
        public PublicarDocumentPersistidoCommand CreatePublicarDocumentPersistidoCommand(OperacaoDocument document)
        {
            var documentPersistido = GetDocumentPersistidoByOperacao(document);
            var publicarDocumentPersistidoCommand = new PublicarDocumentPersistidoCommand
            {
                Message = JsonConvert.SerializeObject(documentPersistido)
            };
            return publicarDocumentPersistidoCommand;
        }
        public OperacaoDocument DeserializeDocument(string message)
        {
            return JsonConvert.DeserializeObject<OperacaoDocument>(message);
        }
        private DocumentPersistido GetDocumentPersistidoByOperacao(OperacaoDocument document)
        {
            return new DocumentPersistido
            {
                DocumentCollection = "posicao",
                Id = document.Id
            };
        }
    }
}
