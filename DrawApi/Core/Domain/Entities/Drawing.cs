using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Exceptions;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.ValueObjects;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.DomainEvents;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities
{
    public class Drawing : BaseEntity
    {
        public DateTime StartDate { get; private set; }
        public virtual IEnumerable<Number> Numbers { get; private set; }
        public bool Active { get; private set; }
        public bool Completed { get; private set; }
        public DateTime NumbersAddedAt { get; private set; }

        protected Drawing(Guid id) : base(id)
        {
            InitalizeNumbers();
        }

        protected Drawing(Guid id, DateTime startDate) : base(id)
        {
            SetStartDate(startDate);
            InitalizeNumbers();
            Active = false;
            Completed = false;
            Id = id;
        }

        protected void DrawingCompleted()
            => AddEvent(new DrawingCompleted(Id));

        protected void SetActive(bool active)
            => Active = active;

        protected void SetCompleted(bool completed)
            => Completed = completed;

        protected void SetStartDate(DateTime startDate)
            => StartDate = startDate;

        public void AddNumber(Number number)
        {
            if (number == null)
                throw new DomainException($"Cannot add null value for {nameof(Numbers)}!");
            if (Numbers.Count() >= 9)
                throw new DomainException($"Only 9 {nameof(Numbers)} are allowed!");
            if (Numbers.Where(x => x.Extra).Count() >= 2 && number.Extra)
                throw new DomainException($"Only 2 Extra {nameof(Numbers)} are allowed!");
            (Numbers as ICollection<Number>).Add(number);
        }

        public void SetNumbersAddedAtDate() => NumbersAddedAt = DateTime.UtcNow;

        public bool HasNumbers() => Numbers.Any();

        protected void InitalizeNumbers() => Numbers = new List<Number>();

        public static Drawing CreateDrawing(DateTime startDate)
                => new Drawing(Guid.NewGuid(), startDate);

        public void Activate() => Active = true;
        public void Deactivate() => Active = false;

        public void Complete()
        {
            Completed = true;
            SetStartDate(DateTime.UtcNow);
            DrawingCompleted();
        } 

        public void UpdateStartDate(DateTime startDate)
        {
            if (startDate < DateTime.UtcNow)
                throw new DomainException("StartDate cannot be lower than today!");
            SetStartDate(startDate);
        }
    }
}
