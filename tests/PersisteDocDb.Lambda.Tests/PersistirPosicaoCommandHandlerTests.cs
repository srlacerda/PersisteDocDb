using MediatR;
using Moq;
using Newtonsoft.Json;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Domain.Base;
using PersisteDocDb.Lambda.Domain.Entities;
using PersisteDocDb.Lambda.Infrastructure.Factory;
using PersisteDocDb.Lambda.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace PersisteDocDb.Lambda.Tests
{
    [Collection(nameof(PosicaoCollection))]
    public class PersistirPosicaoCommandHandlerTests
    {
        private readonly PosicaoTestsFixture _posicaoTestsFixture;
        private readonly PersistirPosicaoCommandHandler _persistirPosicaoCommandHandler;

        public PersistirPosicaoCommandHandlerTests(PosicaoTestsFixture posicaoTestsFixture)
        {
            _posicaoTestsFixture = posicaoTestsFixture;
            _persistirPosicaoCommandHandler = _posicaoTestsFixture.ObterPersistirPosicaoCommandHandler();
        }

        [Fact(DisplayName ="Null Request")]
        [Trait("Categoria","Posicao - Command Handler")]
        public void PersistirPosicao_InserirPosicao_NullRequest()
        {
            // Arrange
            var persistirPosicaoCommand = new PersistirPosicaoCommand();

            // Act & Assert
            var exception =
                Assert.ThrowsAsync<ArgumentException>(() => _persistirPosicaoCommandHandler.Handle(persistirPosicaoCommand, CancellationToken.None)).Result;

            Assert.Contains("Request Cant Be Null", exception.Message);

        }

        [Fact(DisplayName = "Posicao Invalida")]
        [Trait("Categoria", "Posicao - Command Handler")]
        public void PersistirPosicao_InserirPosicao_PosicaoInvalida()
        {
            // Arrange
            var posicaoDocument = _posicaoTestsFixture.GerarPosicaoDocumentInvalida();
            var message = JsonConvert.SerializeObject(posicaoDocument);
            var persistirPosicaoCommand = new PersistirPosicaoCommand
            {
                Message = message
            };

            _posicaoTestsFixture.Mocker.GetMock<IDocumentFactory<PosicaoDocument>>().Setup(p => p.DeserializeDocument(message))
                .Returns(posicaoDocument);

            // Act & Assert
            var exception =
                Assert.ThrowsAsync<ArgumentException>(() => _persistirPosicaoCommandHandler.Handle(persistirPosicaoCommand, CancellationToken.None)).Result;

            Assert.Contains("DataPosicao is required", exception.Message);
            Assert.Contains("CodigoFatura is required", exception.Message);
            Assert.Contains("Mercado does not exists", exception.Message);

        }

        [Fact(DisplayName = "Inserir Posicao com Sucesso")]
        [Trait("Categoria", "Posicao - Command Handler")]
        public void PersistirPosicao_InserirPosicao_DeveExecutarComSucesso()
        {
            // Arrange
            var posicaoDocument = _posicaoTestsFixture.GerarPosicaoDocumentValida();
            var message = JsonConvert.SerializeObject(posicaoDocument);
            var persistirPosicaoCommand = new PersistirPosicaoCommand
            {
                Message = message
            };

            _posicaoTestsFixture.Mocker.GetMock<IDocumentFactory<PosicaoDocument>>().Setup(p => p.DeserializeDocument(message))
                .Returns(posicaoDocument);

            _posicaoTestsFixture.Mocker.GetMock<IDocumentRepository<PosicaoDocument>>().Setup(p => p.PersistirDocumentReplaceOne(posicaoDocument))
                .Returns(0);

            _posicaoTestsFixture.Mocker.GetMock<IMediator>().Setup(m => m.Send(It.IsAny<PublicarDocumentPersistidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { Sucess = true });

            // Act
            var result = _persistirPosicaoCommandHandler.Handle(persistirPosicaoCommand, CancellationToken.None);

            // Assert
            _posicaoTestsFixture.Mocker.GetMock<IDocumentRepository<PosicaoDocument>>().Verify(r => r.PersistirDocumentReplaceOne(posicaoDocument), Times.Once);
            _posicaoTestsFixture.Mocker.GetMock<IMediator>().Verify(m => m.Send(It.IsAny<PublicarDocumentPersistidoCommand>(), CancellationToken.None), Times.Once);
            Assert.True(result.Result.Sucess);
        }

        [Fact(DisplayName = "Alterar Posicao com Sucesso")]
        [Trait("Categoria", "Posicao - Command Handler")]
        public void PersistirPosicao_AlterarPosicao_DeveExecutarComSucesso()
        {
            // Arrange
            var posicaoDocument = _posicaoTestsFixture.GerarPosicaoDocumentValida();
            var message = JsonConvert.SerializeObject(posicaoDocument);
            var persistirPosicaoCommand = new PersistirPosicaoCommand
            {
                Message = message
            };

            _posicaoTestsFixture.Mocker.GetMock<IDocumentFactory<PosicaoDocument>>().Setup(p => p.DeserializeDocument(message))
                .Returns(posicaoDocument);

            _posicaoTestsFixture.Mocker.GetMock<IDocumentRepository<PosicaoDocument>>().Setup(p => p.PersistirDocumentReplaceOne(posicaoDocument))
                .Returns(1);

            _posicaoTestsFixture.Mocker.GetMock<IMediator>().Setup(m => m.Send(It.IsAny<PublicarDocumentPersistidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result { Sucess = true });

            // Act
            var result = _persistirPosicaoCommandHandler.Handle(persistirPosicaoCommand, CancellationToken.None);

            // Assert
            _posicaoTestsFixture.Mocker.GetMock<IDocumentRepository<PosicaoDocument>>().Verify(r => r.PersistirDocumentReplaceOne(posicaoDocument), Times.Once);
            _posicaoTestsFixture.Mocker.GetMock<IMediator>().Verify(m => m.Send(It.IsAny<PublicarDocumentPersistidoCommand>(), CancellationToken.None), Times.Once);
            Assert.True(result.Result.Sucess);

        }
    }
}
