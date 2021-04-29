using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.DTO
{
    public class ScheduleDTO
    {
        public Guid Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public IEnumerable<int> Days { get; set; }
    }
}
