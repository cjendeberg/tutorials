using AutoMapper;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.ValueObjects;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.DTO;
using System.Linq;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(config =>
            {
                config.CreateMap<Number, NumberDTO>();
                config.CreateMap<Drawing, DrawingDTO>();

                config.CreateMap<Schedule, ScheduleDTO>()
                    .ForMember(d => d.Days, s => s.MapFrom(x => x.Days.Select(y=>y.DayOfWeek)));
            }).CreateMapper();
    }
}
