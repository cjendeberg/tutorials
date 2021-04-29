using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Messages.Commands;

namespace Zero99Lotto.SRC.Common.Dispatchers
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
