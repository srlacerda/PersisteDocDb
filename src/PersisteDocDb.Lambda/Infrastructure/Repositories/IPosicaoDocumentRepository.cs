using PersisteDocDb.Lambda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersisteDocDb.Lambda.Infrastructure.Repositories
{
    public interface IPosicaoDocumentRepository
    {
        Task InserirPosicaoAsync(PosicaoDocument posicaoDocument);
    }
}
