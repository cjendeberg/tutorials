using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Exceptions;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure
{
    public class ServiceException : DrawException
    {
        public ServiceException()
        {
        }

        public ServiceException(string message) : base(message)
        {
        }

        public ServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
