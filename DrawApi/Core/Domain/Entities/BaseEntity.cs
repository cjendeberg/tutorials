using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.Events;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities
{
    public abstract class BaseEntity : IEntity
    {
        private readonly ConcurrentDictionary<Type, IDomainEvent> _events =
            new ConcurrentDictionary<Type, IDomainEvent>();

        public Guid Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public IEnumerable<IDomainEvent> Events => _events.Values;

        public BaseEntity(Guid id)
        {
            Id = id;
            CreatedDate = DateTime.UtcNow;
        }

        protected void AddEvent(IDomainEvent @event)
        {
            var eventType = @event.GetType();
            if (_events.ContainsKey(eventType))
                return;

            _events[eventType] = @event;
        }
           

        protected void ClearEvents()
            => _events.Clear();
    }
}
