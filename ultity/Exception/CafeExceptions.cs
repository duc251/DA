using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Exceptions
{
    public class CafeException : Exception
    {
        public CafeException()
        {

        }
        public CafeException(string message) : base(message)
        {

        }
        public CafeException(string message, Exception inner):
            base(message, inner)
        {

        }
    }
}
