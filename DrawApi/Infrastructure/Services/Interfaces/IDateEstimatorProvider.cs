using System;
using Zero99Lotto.SRC.Common.Services;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces
{
    public interface IDateEstimatorProvider : IService, IDisposable
    {
        DateTime GetClosestDate(int[] daysOfWeek, TimeSpan time);
        DateTime EstimateDate(int day, TimeSpan time);
    }
}
