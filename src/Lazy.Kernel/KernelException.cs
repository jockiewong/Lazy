using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.Kernel
{
    public class KernelException : Exception
    {
        public int ErrorCode { get; private set; }

        public KernelException(string msg) : base(msg)
        {

        }

        public KernelException(string msg, Exception innerException) : base(msg, innerException)
        {

        }

        public KernelException(int errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
