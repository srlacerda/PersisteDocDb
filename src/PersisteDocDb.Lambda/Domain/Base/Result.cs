﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PersisteDocDb.Lambda.Domain.Base
{
    public class Result : IResult
    {
        public int ResponseCode { get; set; }
        public object Content { get; set; }
        public bool Sucess { get; set; }
        public Exception Exception { get; set; }
    }
}
