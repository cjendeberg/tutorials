using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Exceptions
{
    public class DomainException : DrawException
    {
        public DomainException()
        {
        }

        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
