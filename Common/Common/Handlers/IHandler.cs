using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Exceptions;

namespace Zero99Lotto.SRC.Common.Handlers
{
    public interface IHandler
    {
        IHandler Handle(Func<Task> handle);
        IHandler OnSuccess(Func<Task> onSuccess);
        IHandler OnError(Func<Exception, Task> onError, bool rethrow = false);
        IHandler OnCustomError(Func<Zero99LottoException, Task> onCustomError, bool rethrow = false);
        IHandler Always(Func<Task> always);
        Task ExecuteAsync();
    }
}
