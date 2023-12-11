using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Common.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException() { }

        public InvalidRequestException(string message) : base(message) { }

        public InvalidRequestException(string message, Exception ex) : base(message, ex) { }
    }
}
