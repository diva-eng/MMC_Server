using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMC
{
    public class MMCException : Exception
    {
        public MMCException()
        {
        }

        public MMCException(string message)
            : base(message)
        {
        }

        public MMCException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
