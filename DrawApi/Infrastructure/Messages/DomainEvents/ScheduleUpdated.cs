using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.Events;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.DomainEvents
{
    public class ScheduleUpdated : IDomainEvent
    {
        public Guid Id { get; }

        public ScheduleUpdated(Guid id)
            => Id = id;
    }
}
