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
        public PersisteOperacaoCommandHandler PersisteOperacaoCommandHandler;
        public AutoMocker Mocker;

        public PersisteOperacaoCommandHandler ObterPersisteOperacaoCommandHandler()
        {
            Mocker = new AutoMocker();
            PersisteOperacaoCommandHandler = Mocker.CreateInstance<PersisteOperacaoCommandHandler>();
            return PersisteOperacaoCommandHandler;
        }

        public void Dispose()
        {
        }
    }
}
