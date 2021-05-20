using Moq.AutoMock;
using Pacote.Core.Domain.Model.Enums;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PersisteDocDb.Lambda.Tests
{
    [CollectionDefinition(nameof(PosicaoCollection))]
    
    public class PosicaoCollection: ICollectionFixture<PosicaoTestsFixture> 
    {}

    public class PosicaoTestsFixture : IDisposable
    {
        public PersistePosicaoCommandHandler PersistePosicaoCommandHandler;
        public AutoMocker Mocker;

        public PersistePosicaoCommandHandler ObterPersistePosicaoCommandHandler()
        {
            Mocker = new AutoMocker();
            PersistePosicaoCommandHandler = Mocker.CreateInstance<PersistePosicaoCommandHandler>();
            return PersistePosicaoCommandHandler;
        }

        public PosicaoDocument GerarPosicaoDocumentValida()
        {
            return new PosicaoDocument
            {
                Mercado = MercadoEnum.DIGITAL_ASSETS,
                CodigoFatura = Guid.NewGuid().ToString(),
                DataPosicao = DateTime.Now.AddDays(-1),
                CodigoSistema = 25,
                Preco = 10
            };
        }

        public PosicaoDocument GerarPosicaoDocumentInvalida()
        {
            return new PosicaoDocument
            {
                Preco = 1000
            };
        }

        public void Dispose()
        {
        }
    }
}
