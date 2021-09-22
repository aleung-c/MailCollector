using System;
using System.Collections.Generic;
using System.Text;

namespace AleungcMailCollector.Exceptions
{
    class CouldNotOpenPageException : Exception
    {
        public CouldNotOpenPageException(string message) : base(message) {}
    }
}
