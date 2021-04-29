using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Zero99Lotto.SRC.Common.Exceptions
{
    public class Zero99LottoException : Exception
    {
        public Zero99LottoException()
        {
        }

        public Zero99LottoException(string message) : base(message)
        {
        }

        public Zero99LottoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public Zero99LottoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
