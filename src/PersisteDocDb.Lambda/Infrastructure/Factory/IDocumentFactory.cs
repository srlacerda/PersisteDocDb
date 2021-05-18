using Pacote.Core.Domain.Util.DocumentDB;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersisteDocDb.Lambda.Infrastructure.Factory
{
    public interface IDocumentFactory<T> where T : IDocument
    {
        T DeserializeDocument(string message);
        PublicarDocumentPersistidoCommand CreatePublicarDocumentPersistidoCommand(T document);
    }
}
