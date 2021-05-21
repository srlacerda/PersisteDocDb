using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Infrastructure.Messaging;
using System;
using System.Threading;
using Xunit;

namespace PersisteDocDb.Lambda.Tests
{
    [Collection(nameof(DocumentPersistidoCollection))]
    public class PublicarDocumentPersistidoCommandHandlerTests
    {
        private readonly DocumentPersistidoTestsFixture _documentPersistidoTestsFixture;
        private readonly PublicarDocumentPersistidoCommandHandler _publicarDocumentPersistidoCommandHandler;

        public PublicarDocumentPersistidoCommandHandlerTests(DocumentPersistidoTestsFixture documentPersistidoTestsFixture)
        {
            _documentPersistidoTestsFixture = documentPersistidoTestsFixture;
            _publicarDocumentPersistidoCommandHandler = _documentPersistidoTestsFixture.ObterPublicarDocumentPersistidoCommandHandler();
        }

        [Fact(DisplayName = "Null Request")]
        [Trait("Categoria", "DocumentPersistido - Command Handler")]
        public void PublicarDocumentPersisitido_Publicar_NullRequest()
        {
            // Arrange
            var publicarDocumentPersistidoCommand = new PublicarDocumentPersistidoCommand();

            // Act & Assert
            var exception =
                Assert.ThrowsAsync<ArgumentException>(() => _publicarDocumentPersistidoCommandHandler.Handle(publicarDocumentPersistidoCommand, CancellationToken.None)).Result;

            Assert.Contains("Request Cant Be Null", exception.Message);
        }

        [Fact(DisplayName = "DocumentPersistido Invalido")]
        [Trait("Categoria", "DocumentPersistido - Command Handler")]
        public void PublicarDocumentPersisitido_Publicar_DocumentPersistidoInvalido()
        {
            // Arrange
            var documentPersistido = _documentPersistidoTestsFixture.GerarDocumentPersistidoInvalido();
            var message = JsonConvert.SerializeObject(documentPersistido);
            var publicarDocumentPersistidoCommand = new PublicarDocumentPersistidoCommand
            {
                Message = message
            };

            // Act & Assert
            var exception =
                Assert.ThrowsAsync<ArgumentException>(() => _publicarDocumentPersistidoCommandHandler.Handle(publicarDocumentPersistidoCommand, CancellationToken.None)).Result;

            Assert.Contains("Id is required", exception.Message);
            Assert.Contains("DocumentCollection is required", exception.Message);
            Assert.Contains("Mercado does not exists", exception.Message);
        }

        [Fact(DisplayName ="DocumentPersistido Publicado com Sucesso")]
        [Trait("Categoria","DocumentPersistido - Command Handler")]
        public void PublicarDocumentPersisitido_Publicar_DeveExecutarComSucesso()
        {
            // Arrange
            var queue = _documentPersistidoTestsFixture.GerarQueueValida();
            var documentPersistido = _documentPersistidoTestsFixture.GerarDocumentPersistidoValido();
            var messsage = JsonConvert.SerializeObject(documentPersistido);
            var publicarDocumentPersistidoCommand = new PublicarDocumentPersistidoCommand
            {
                Message = messsage
            };

            _documentPersistidoTestsFixture.Mocker.GetMock<IAmazonSqsClientHelper>()
                .Setup(a => a.GetSqsPublicarDocumentPersistidoByMercado(
                        documentPersistido.Mercado,
                        It.IsAny<IConfiguration>()))
                .Returns(queue);

            _documentPersistidoTestsFixture.Mocker.GetMock<IAmazonSqsClientHelper>()
                .Setup(a => a.SenMessageAsync(queue, messsage))
                .Returns(_documentPersistidoTestsFixture.GerarSqsSendMessageResponseOk);

            // Act
            var result = _publicarDocumentPersistidoCommandHandler.Handle(publicarDocumentPersistidoCommand, CancellationToken.None);

            // Assert
            _documentPersistidoTestsFixture.Mocker.GetMock<IAmazonSqsClientHelper>().Verify(a => a.SenMessageAsync(queue, messsage), Times.Once);
            Assert.True(result.Result.Sucess);
        }

        [Fact(DisplayName = "Falha na Pubilicaca")]
        [Trait("Categoria", "DocumentPersistido - Command Handler")]
        public void PublicarDocumentPersisitido_Publicar_DeveFalhar()
        {
            // Arrange
            var queue = _documentPersistidoTestsFixture.GerarQueueValida();
            var documentPersistido = _documentPersistidoTestsFixture.GerarDocumentPersistidoValido();
            var messsage = JsonConvert.SerializeObject(documentPersistido);
            var publicarDocumentPersistidoCommand = new PublicarDocumentPersistidoCommand
            {
                Message = messsage
            };

            _documentPersistidoTestsFixture.Mocker.GetMock<IAmazonSqsClientHelper>()
                .Setup(a => a.GetSqsPublicarDocumentPersistidoByMercado(
                        documentPersistido.Mercado,
                        It.IsAny<IConfiguration>()))
                .Returns(queue);

            _documentPersistidoTestsFixture.Mocker.GetMock<IAmazonSqsClientHelper>()
                .Setup(a => a.SenMessageAsync(queue, messsage))
                .Returns(_documentPersistidoTestsFixture.GerarSqsSendMessageResponseNotOk);

            // Act & Assert
            var exception = 
                Assert.ThrowsAsync<Exception>(() =>  _publicarDocumentPersistidoCommandHandler.Handle(publicarDocumentPersistidoCommand, CancellationToken.None)).Result;

            // 
            _documentPersistidoTestsFixture.Mocker.GetMock<IAmazonSqsClientHelper>().Verify(a => a.SenMessageAsync(queue, messsage), Times.Once);
            Assert.Contains("Unable to sent message to queue", exception.Message);
        }

    }
}
