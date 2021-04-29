using System;
using System.Collections.Generic;
using System.Text;

namespace t1
{
    public interface ICommand : IMessage
    {
    }

    public interface ICommandWithResult<T> : ICommand
    {
        T Result { get; set; }
    }
}
