using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Common.Exceptions
{
    public class SaleDateException : Exception
    {
        public SaleDateException()
        {
        }

        public SaleDateException(string message)
            : base(message)
        {
        }

        public SaleDateException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
