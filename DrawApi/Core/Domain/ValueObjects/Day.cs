using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Exceptions;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.ValueObjects
{
    public class Day : ValueObject
    {
        public int DayOfWeek { get; private set; }

        protected Day(int dayOfWeek)
        {
            SetDayOfWeek(dayOfWeek);
        } 

        protected void SetDayOfWeek(int dayOfWeek)
            => DayOfWeek = dayOfWeek < 0 || dayOfWeek > 6 ?
                throw new DomainException($"{nameof(DayOfWeek)} cannot be lower then 0 or higher then 6! " +
                    $"Current value: {dayOfWeek}.")
                : dayOfWeek;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DayOfWeek;
        }

        public static Day CreateDay(int dayOfWeek)
            => new Day(dayOfWeek);
    }
}
