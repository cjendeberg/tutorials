using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.Events;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Dispatchers
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync<T>(params T[] events) where T : IDomainEvent;
    }
}
