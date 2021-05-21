using MediatR;
using Moq;
using Newtonsoft.Json;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Domain.Base;
using PersisteDocDb.Lambda.Domain.Entities;
using PersisteDocDb.Lambda.Infrastructure.Factory;
using PersisteDocDb.Lambda.Infrastructure.Repositories;
using System;
using System.Threading;
using Xunit;

namespace PersisteDocDb.Lambda.Tests
{
    [Collection(nameof(OperacaoCollection))]
    public class PersistirOperacaoCommandHandlerTests
    {
        private readonly OperacaoTestsFixture _operacaoTestsFixture;
        private readonly PersistirOperacaoCommandHandler _persistirOperacaoCommandHandler;

        public PersistirOperacaoCommandHandlerTests(OperacaoTestsFixture operacaoTestsFixture)
        {
            _operacaoTestsFixture = operacaoTestsFixture;
            _persistirOperacaoCommandHandler = _operacaoTestsFixture.ObterPersistirOperacaoCommandHandler();
        }

        [Fact(DisplayName = "Null Request")]
        [Trait("Categoria", "Operacao - Command Handler")]
        public void PersistirOperacao_InserirPosicao_NullRequest()
        {
            // Arrange
            var persistirOperacaoCommand = new PersistirOperacaoCommand();

            // Act & Assert
            var exception =
                Assert.ThrowsAsync<ArgumentException>(() => _persistirOperacaoCommandHandler.Handle(persistirOperacaoCommand, CancellationToken.None)).Result;

            Assert.Contains("Request Cant Be Null", exception.Message);

        }

        [Fact(DisplayName = "Operacao Invalida")]
        [Trait("Categoria", "Operacao - Command Handler")]
        public void PersistirOperacao_InserirPosicao_PosicaoInvalida()
        {
            // Arrange
            var operacaoDocument = _operacaoTestsFixture.GerarOperacaoDocumentInvalida();
            var message = JsonConvert.SerializeObject(operacaoDocument);
            var persistirOperacaoCommand = new PersistirOperacaoCommand
            {
                Message = message
            };

            _operacaoTestsFixture.Mocker.GetMock<IDocumentFactory<OperacaoDocument>>().Setup(p => p.DeserializeDocument(message))
                .Returns(operacaoDocument);

            // Act & Assert
            var exception =
                Assert.ThrowsAsync<ArgumentException>(() => _persistirOperacaoCommandHandler.Handle(persistirOperacaoCommand, CancellationToken.None)).Result;

            Assert.Contains("DataPosicao is required", exception.Message);
            Assert.Contains("CodigoFatura is required", exception.Message);
            Assert.Contains("Mercado does not exists", exception.Message);

        }

        [Fact(DisplayName = "Inserir Operacao com Sucesso")]
        [Trait("Categoria", "Operacao - Command Handler")]
        public void PersistirOperacao_InserirPosicao_DeveExecutarComSucesso()
        {
            // Arrange
            var operacaoDocument = _operacaoTestsFixture.GerarOperacaoDocumentValida();
            var message = JsonConvert.SerializeObject(operacaoDocument);
            var persistirOperacaoCommand = new PersistirOperacaoCommand
            {
                Message = message
            };

            _operacaoTestsFixture.Mocker.GetMock<IDocumentFactory<OperacaoDocument>>().Setup(p => p.DeserializeDocument(message))
                .Returns(operacaoDocument);

            _operacaoTestsFixture.Mocker.GetMock<IDocumentRepository<OperacaoDocument>>().Setup(p => p.PersistirDocumentReplaceOne(operacaoDocument))
                .Returns(0);

            _operacaoTestsFixture.Mocker.GetMock<IMediator>().Setup(m => m.Send(It.IsAny<PublicarDocumentPersistidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { Sucess = true });

            // Act
            var result = _persistirOperacaoCommandHandler.Handle(persistirOperacaoCommand, CancellationToken.None);

            // Assert
            _operacaoTestsFixture.Mocker.GetMock<IDocumentRepository<OperacaoDocument>>().Verify(r => r.PersistirDocumentReplaceOne(operacaoDocument), Times.Once);
            _operacaoTestsFixture.Mocker.GetMock<IMediator>().Verify(m => m.Send(It.IsAny<PublicarDocumentPersistidoCommand>(), CancellationToken.None), Times.Once);
            Assert.True(result.Result.Sucess);
        }

        [Fact(DisplayName = "Alterar Operacao com Sucesso")]
        [Trait("Categoria", "Operacao - Command Handler")]
        public void PersistirOperacao_AlterarPosicao_DeveExecutarComSucesso()
        {
            // Arrange
            var operacaoDocument = _operacaoTestsFixture.GerarOperacaoDocumentValida();
            var message = JsonConvert.SerializeObject(operacaoDocument);
            var persistirOperacaoCommand = new PersistirOperacaoCommand
            {
                Message = message
            };

            _operacaoTestsFixture.Mocker.GetMock<IDocumentFactory<OperacaoDocument>>().Setup(p => p.DeserializeDocument(message))
                .Returns(operacaoDocument);

            _operacaoTestsFixture.Mocker.GetMock<IDocumentRepository<OperacaoDocument>>().Setup(p => p.PersistirDocumentReplaceOne(operacaoDocument))
                .Returns(1);

            _operacaoTestsFixture.Mocker.GetMock<IMediator>().Setup(m => m.Send(It.IsAny<PublicarDocumentPersistidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { Sucess = true });

            // Act
            var result = _persistirOperacaoCommandHandler.Handle(persistirOperacaoCommand, CancellationToken.None);

            // Assert
            _operacaoTestsFixture.Mocker.GetMock<IDocumentRepository<OperacaoDocument>>().Verify(r => r.PersistirDocumentReplaceOne(operacaoDocument), Times.Once);
            _operacaoTestsFixture.Mocker.GetMock<IMediator>().Verify(m => m.Send(It.IsAny<PublicarDocumentPersistidoCommand>(), CancellationToken.None), Times.Once);
            Assert.True(result.Result.Sucess);

        }
    }
}
