using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Exceptions;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.ValueObjects;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.DomainEvents;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities
{
    public class Schedule : BaseEntity, IAggregateRoot
    {
        public TimeSpan StartTime { get; private set; }
        public virtual IEnumerable<Day> Days { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public virtual IEnumerable<Drawing> Drawings { get; private set; }

        protected Schedule(Guid id) : base(id)
        {
            InitializeDrawings();
            InitialieDays();
        }

        protected Schedule(Guid id, TimeSpan startTime, int[] daysOfWeek) : base(id)
        {
            InitializeDrawings();
            InitialieDays();
            SetStartTime(startTime);
            SetDaysOfWeek(daysOfWeek);
            UpdatedAt = DateTime.UtcNow;
            ScheduleCreated();
        }

        protected void SetStartTime(TimeSpan startTime)
            => StartTime = startTime.Hours < 0 || startTime.Days != 0 ?
                throw new DomainException($"{nameof(StartTime)} invalid format!") : startTime;

        protected void SetDaysOfWeek(int[] daysOfWeek)
        {
            if (!daysOfWeek.Any())
                throw new DomainException($"{nameof(Days)} must have atleast one value!");
            if (daysOfWeek.Count() > 7)
                throw new DomainException($"{nameof(Days)} cannot have more then 7!");
            foreach (var digit in daysOfWeek)
            {
                if (Days.Any(x => x.DayOfWeek == digit))
                    throw new DomainException($"{nameof(Days)} already contains {digit}!");
                (Days as ICollection<Day>).Add(Day.CreateDay(digit));
            }
        }

        protected void ScheduleCreated()
            => AddEvent(new ScheduleCreated(Id));

        protected void ScheduleUpdated()
            => AddEvent(new ScheduleUpdated(Id));

        public void AddDrawing(Drawing drawing)
            => (Drawings as ICollection<Drawing>)
                .Add(drawing ?? throw new DomainException($"Cannot add null value for {nameof(Drawings)}!"));

        protected void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
            ScheduleUpdated();
        }

        public Drawing GetActiveDrawing()
        {
            var drawing = Drawings.FirstOrDefault(x => x.Active);
            if (drawing == null)
                throw new DomainException($"Schedule doesn't have active drawing!");
            return drawing;
        }

        public bool HasActiveDrawing() => Drawings.Any(x => x.Active);

        public void MarkAsActive(Drawing drawing)
        {
            if(Drawings.Any(x => x.Active))
                throw new DomainException($"There is already active drawing!");
            var drawingForActivation = Drawings.FirstOrDefault(x => x.Id == drawing.Id);
            if (drawingForActivation != null)
                drawingForActivation.Activate();
            else
                throw new DomainException($"Drawing of Id: {drawing.Id} not found in collection for activation!");
        }

        protected void InitializeDrawings() => Drawings = new List<Drawing>();
        protected void InitialieDays() => Days = new List<Day>();

        public static Schedule CreateSchedule(TimeSpan startTime, int[] daysOfWeek)
                => new Schedule(Guid.NewGuid(), startTime, daysOfWeek);

        public void UpdateTime(TimeSpan time)
        {
            SetStartTime(time);
            SetUpdatedAt();
        }

        public void UpdateDays(int[] daysOfWeek)
        {
            InitialieDays();
            SetDaysOfWeek(daysOfWeek);
            SetUpdatedAt();
        }

    }
}
