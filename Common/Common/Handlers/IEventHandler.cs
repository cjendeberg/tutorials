using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
