using System;
using System.Collections.Generic;
using System.Text;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Messages.Commands;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Bus
{
    public class BusSubscriber : IBusSubscriber
    {
        private readonly IBusClient _busClient;

        public BusSubscriber(IBusClient busClient) => _busClient = busClient;

        public IBusSubscriber SubscribeCommand<TCommand>() where TCommand : ICommand
            => _busClient.SubscribeCommand<TCommand>();

        public IBusSubscriber SubscribeEvent<TEvent>() where TEvent : IEvent
            => _busClient.SubscribeEvent<TEvent>();
    }
}
