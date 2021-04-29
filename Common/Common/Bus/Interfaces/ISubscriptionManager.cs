using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Messages;
using Zero99Lotto.SRC.Common.Messages.Commands;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Bus.Interfaces
{
    public interface ISubscriptionManager
    {
        Task SubscribeCommandAsync<TCommand>() where TCommand : ICommand;
        Task SubscribeEventAsync<TEvent>() where TEvent : IEvent;
        bool HasSubscription<TMessage>() where TMessage : IMessage;
        Type GetRegisteredMessageType(string messageName);
        string GetMessageName<TMessage>();
        bool BelongsToCommands(string messageName);
        bool BelongsToEvents(string messageName);
        void Clear();
    }
}
