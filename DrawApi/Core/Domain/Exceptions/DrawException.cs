using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Exceptions;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Exceptions
{
    public class DrawException : Zero99LottoException
    {
        public DrawException()
        {
        }

        public DrawException(string message) : base(message)
        {
        }

        public DrawException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DrawException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
