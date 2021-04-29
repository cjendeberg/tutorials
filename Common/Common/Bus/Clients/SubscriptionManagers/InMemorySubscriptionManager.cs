using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Messages;
using Zero99Lotto.SRC.Common.Messages.Commands;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Bus.Clients.SubscriptionManagers
{
    public class InMemorySubscriptionManager : ISubscriptionManager
    {
        private readonly Dictionary<string, Type> _eventsTypes;
        private readonly Dictionary<string, Type> _commandsTypes;

        public InMemorySubscriptionManager()
        {
            _eventsTypes = new Dictionary<string, Type>();
            _commandsTypes = new Dictionary<string, Type>();
        }

        public Type GetRegisteredMessageType(string messageName)
        {
            Type result = null;
            if (!_eventsTypes.TryGetValue(messageName, out result))
                _commandsTypes.TryGetValue(messageName, out result);

            return result;
        }

        public bool HasSubscription<TMessage>() where TMessage : IMessage
        {
            var type = typeof(TMessage);
            return _eventsTypes.Any(x => x.Value.Equals(type)) || _commandsTypes.Any(x => x.Value.Equals(type));
        }

        public async Task SubscribeCommandAsync<TCommand>() where TCommand : ICommand
        {
            _commandsTypes.Add(GetMessageName<TCommand>(), typeof(TCommand));
            await Task.CompletedTask;
        }

        public async Task SubscribeEventAsync<TEvent>() where TEvent : IEvent
        {
            _eventsTypes.Add(GetMessageName<TEvent>(),typeof(TEvent));
            await Task.CompletedTask;
        }

        public string GetMessageName<TMessage>() => typeof(TMessage).Name;

        public bool BelongsToCommands(string messageName)
            => _commandsTypes.Keys.Any(x => x.Equals(messageName));

        public bool BelongsToEvents(string messageName)
            => _eventsTypes.Keys.Any(x => x.Equals(messageName));

        public void Clear()
        {
            _eventsTypes.Clear();
            _commandsTypes.Clear();
        }
    }
}
