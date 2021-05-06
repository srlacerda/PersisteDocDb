using System;
using System.Collections.Generic;
using System.Text;

namespace PersisteDocDb.Lambda.Infrastructure.Logging
{
    public interface ILogger
    {
        void Error(string message);
        void Error(Exception exception, string message);
        void Info(string message);
        void Warning(string message);
        void Debug(string message);
    }
}
