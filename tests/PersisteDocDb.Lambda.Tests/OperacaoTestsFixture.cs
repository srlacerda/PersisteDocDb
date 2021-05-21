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
    [CollectionDefinition(nameof(OperacaoCollection))]

    public class OperacaoCollection : ICollectionFixture<OperacaoTestsFixture>
    { }
    public class OperacaoTestsFixture : IDisposable
    {
        public PersistirOperacaoCommandHandler PersistirOperacaoCommandHandler;
        public AutoMocker Mocker;

        public PersistirOperacaoCommandHandler ObterPersistirOperacaoCommandHandler()
        {
            Mocker = new AutoMocker();
            PersistirOperacaoCommandHandler = Mocker.CreateInstance<PersistirOperacaoCommandHandler>();
            return PersistirOperacaoCommandHandler;
        }

        public OperacaoDocument GerarOperacaoDocumentValida()
        {
            return new OperacaoDocument
            {
                Mercado = MercadoEnum.DIGITAL_ASSETS,
                CodigoFatura = Guid.NewGuid().ToString(),
                DataOperacao = DateTime.Now.AddDays(-1),
                CodigoSistema = 25,
                Preco = 10
            };
        }

        public OperacaoDocument GerarOperacaoDocumentInvalida()
        {
            return new OperacaoDocument
            {
                Preco = 1000
            };
        }

        public void Dispose()
        {
        }
    }
}
