using System;
using System.Collections.Generic;
using System.Text;

namespace PersisteDocDb.Lambda.Domain.Base
{
    public interface IResult
    {
        int ResponseCode { get; set; }
        object Content { get; set; }
        bool Sucess { get; set; }
        Exception Exception { get; set; }
    }
}
