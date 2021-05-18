﻿using Newtonsoft.Json;
using Pacote.Core.Domain.Model.Contratos;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Domain.Entities;

namespace PersisteDocDb.Lambda.Infrastructure.Factory
{
    public class PosicaoFactory : IDocumentFactory<PosicaoDocument>
    {
        public PublicarDocumentPersistidoCommand CreatePublicarDocumentPersistidoCommand(PosicaoDocument document)
        {
            var documentoPublicado = GetDocumentoPublicado(document);
            var publicarDocumentPersistidoCommand = new PublicarDocumentPersistidoCommand
            {
                Message = JsonConvert.SerializeObject(documentoPublicado)
            };
            return publicarDocumentPersistidoCommand;
        }
        public PosicaoDocument DeserializeDocument(string message)
        {
            return JsonConvert.DeserializeObject<PosicaoDocument>(message);
        }
        private DocumentoPublicado GetDocumentoPublicado(PosicaoDocument document)
        {
            return new DocumentoPublicado
            {
                DocumentCollection = "posicao",
                Id = document.Id
            };
        }
    }
}
