using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Exceptions
{
    public class UpdateEntityException : Exception
{
    public UpdateEntityException()
    {
    }

    public UpdateEntityException(string message)
        : base(message)
    {
    }

    public UpdateEntityException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
}