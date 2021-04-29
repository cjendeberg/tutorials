using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.DTO;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.Queries;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class GetScheduleHandler : IQueryHandler<GetSchedule, ScheduleDTO>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IMapper _mapper;

        public GetScheduleHandler(IScheduleRepository scheduleRepository, IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
        }
        public async Task<ScheduleDTO> Handle(GetSchedule query)
        {
            var schedule = await _scheduleRepository.GetAsync().ConfigureAwait(false);
            return _mapper.Map<ScheduleDTO>(schedule);
        }
    }
}
