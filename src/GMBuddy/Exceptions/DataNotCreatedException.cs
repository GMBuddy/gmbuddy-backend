using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMBuddy.Exceptions
{
    public class DataNotCreatedException : Exception
    {

        public DataNotCreatedException(string message) : base(message)
        {
        }

        public DataNotCreatedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
