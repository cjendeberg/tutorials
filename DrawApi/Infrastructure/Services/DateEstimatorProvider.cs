using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services
{
    public class DateEstimatorProvider : IDateEstimatorProvider
    {
        public DateTime ActualDate => _injectedDateTime ?? DateTime.UtcNow;
        private DateTime? _injectedDateTime;
        private const int intervalTime = 60;
        private const int days_in_week = 7;    

        public DateTime GetClosestDate(int[] daysOfWeek, TimeSpan time)
        {
            var estimatedDays = new List<DateTime>();
            foreach (var day in daysOfWeek)
                estimatedDays.Add(EstimateDate(day, time));
            return estimatedDays.OrderBy(x => Math.Abs((x - DateTime.UtcNow).Ticks)).First();
        }

        /*
         * Returns the next future Date that occur on the weekday indicated by 'day'.
         * 0 (sunday) to 6 (saturday). The Time part is set to 'time'.
         * Note that the current date is only chosen when the 'time' is at least one
         * hour in the future!
        */
        public DateTime EstimateDate(int day, TimeSpan time)
        {
            var closestDateTime = new DateTime(ActualDate.Year, ActualDate.Month, ActualDate.Day,
                    time.Hours, time.Minutes, time.Seconds);

            // Loop over all future days from today (inclusive) and one week in the future (inclusive)
            // The day one week in the future is chosen if the 'time' is less than one hour from now.
            for (int i = 0; i <= days_in_week; i++)
            {
                if(i > 0)
                    closestDateTime = closestDateTime.AddDays(1);              
                var differenceHours = (int)(closestDateTime - ActualDate).TotalHours;

                if (differenceHours > 0 && day==(int)closestDateTime.DayOfWeek)
                    return new DateTime(closestDateTime.Year, closestDateTime.Month,
                       closestDateTime.Day, time.Hours, time.Minutes, time.Seconds);
            }
            throw new ServiceException($"Cannot estimate date, invalid format - ActualDate: {ActualDate}, ProvidedDay: {day}, ProvidedTime: {time}");
        }

        public IDisposable InjectActualDateTime(DateTime actualDate)
        {
            _injectedDateTime = actualDate;
            return new DateEstimatorProvider();
        }

        public void Dispose() => _injectedDateTime = null;
    }
}
