using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.DTO
{
    public class DrawingDTO
    {
        public DateTime StartDate { get; set; }
        public IEnumerable<NumberDTO> Numbers { get; set; }
        public bool Active { get; set; }
        public bool Completed { get; set; }
        public Guid Id { get; set; }
    }
}
