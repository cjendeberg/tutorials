using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.Events;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        Task HandleAsync(T @event);
    }
}
