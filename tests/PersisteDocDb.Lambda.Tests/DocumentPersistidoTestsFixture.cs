using Moq.AutoMock;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PersisteDocDb.Lambda.Tests
{
    [CollectionDefinition(nameof(DocumentPersistidoCollection))]

    public class DocumentPersistidoCollection : ICollectionFixture<DocumentPersistidoTestsFixture>
    { }
    public class DocumentPersistidoTestsFixture : IDisposable
    {
        public PublicarDocumentPersistidoCommandHandler PublicarDocumentPersistidoCommandHandler;
        public AutoMocker Mocker;

        public PublicarDocumentPersistidoCommandHandler ObterPublicarDocumentPersistidoCommandHandler()
        {
            Mocker = new AutoMocker();
            PublicarDocumentPersistidoCommandHandler = Mocker.CreateInstance<PublicarDocumentPersistidoCommandHandler>();
            return PublicarDocumentPersistidoCommandHandler;
        }


        public void Dispose()
        {
        }
    }
}
