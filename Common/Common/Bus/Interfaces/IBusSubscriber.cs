using System;
using System.Collections.Generic;
using System.Text;
using Zero99Lotto.SRC.Common.Messages.Commands;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Bus.Interfaces
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>() where TCommand : ICommand;
        IBusSubscriber SubscribeEvent<TEvent>() where TEvent : IEvent;
    }
}
