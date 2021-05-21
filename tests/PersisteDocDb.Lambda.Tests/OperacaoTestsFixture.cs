using Moq.AutoMock;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
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

        public void Dispose()
        {
        }
    }
}
