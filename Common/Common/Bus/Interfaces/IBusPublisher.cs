using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Messages.Commands;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Bus.Interfaces
{
    public interface IBusPublisher
    {
        Task SendAsync<TCommand>(TCommand command)
           where TCommand : ICommand;

        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}
