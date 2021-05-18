using Pacote.Core.Domain.Util.DocumentDB;
using PersisteDocDb.Lambda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersisteDocDb.Lambda.Infrastructure.Repositories
{
    public interface IDocumentRepository<T> where T : IDocument
    {
        //Task InserirPosicaoAsync(PosicaoDocument posicaoDocument);
        long PersisteDocumentReplaceOne(T document);
    }
}
