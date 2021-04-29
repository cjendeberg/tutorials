using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Bus.Interfaces;
using Zero99Lotto.SRC.Common.Messages.Commands;
using Zero99Lotto.SRC.Common.Messages.Events;

namespace Zero99Lotto.SRC.Common.Bus
{
    public class BusPublisher : IBusPublisher
    {
        private readonly IBusClient _busClient;

        public BusPublisher(IBusClient busClient) => _busClient = busClient;

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
           => await _busClient.PublishAsync(@event);

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
         => await _busClient.SendAsync(command);
    }
}
