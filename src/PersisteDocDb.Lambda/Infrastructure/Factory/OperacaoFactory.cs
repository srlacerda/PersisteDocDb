using Newtonsoft.Json;
using Pacote.Core.Domain.Model.Contratos;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Domain.Entities;

namespace PersisteDocDb.Lambda.Infrastructure.Factory
{
    public class OperacaoFactory : IDocumentFactory<OperacaoDocument>
    {
        public PublicarDocumentPersistidoCommand CreatePublicarDocumentPersistidoCommand(OperacaoDocument document)
        {
            var documentoPublicado = GetDocumentoPublicado(document);
            var publicarDocumentPersistidoCommand = new PublicarDocumentPersistidoCommand
            {
                Message = JsonConvert.SerializeObject(documentoPublicado)
            };
            return publicarDocumentPersistidoCommand;
        }
        public OperacaoDocument DeserializeDocument(string message)
        {
            return JsonConvert.DeserializeObject<OperacaoDocument>(message);
        }
        private DocumentoPublicado GetDocumentoPublicado(OperacaoDocument document)
        {
            return new DocumentoPublicado
            {
                DocumentCollection = "posicao",
                Id = document.Id
            };
        }
    }
}
