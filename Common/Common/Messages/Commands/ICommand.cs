using System;
using System.Collections.Generic;
using System.Text;

namespace Zero99Lotto.SRC.Common.Messages.Commands
{
    public interface ICommand : IMessage
    {
    }

    public interface ICommandWithResult<T> : ICommand
    {
        T Result { get; set; }
    }
}
