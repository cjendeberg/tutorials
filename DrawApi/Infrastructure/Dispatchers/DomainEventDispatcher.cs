using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.Events;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Dispatchers
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IComponentContext _context;

        public DomainEventDispatcher(IComponentContext context)
           => _context = context;

        public async Task DispatchAsync<T>(params T[] events) where T : IDomainEvent
        {
            foreach (var @event in events)
            {
                if (@event == null)
                    throw new ArgumentNullException(nameof(@event), "Event can not be null.");

                var eventType = @event.GetType();
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

                dynamic handler = _context.Resolve(handlerType);

                await handler.HandleAsync((dynamic)@event);

            }
        }
    }
}
