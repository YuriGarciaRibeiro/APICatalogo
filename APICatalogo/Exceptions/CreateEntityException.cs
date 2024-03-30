using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Exceptions
{
    public class CreateEntityException : Exception
    {
        public CreateEntityException()
        {
        }

        public CreateEntityException(string message)
            : base(message)
        {
        }

        public CreateEntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}