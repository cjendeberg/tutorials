using AutoMapper;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Handlers;
using Zero99Lotto.SRC.Common.Services;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.DTO;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Messages.Queries;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Handlers
{
    public class GetActiveDrawingHandler : IQueryHandler<GetActiveDrawing, DrawingDTO>
    {
        private readonly IDrawingRepository _drawingRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCacheService _distributedCacheService;

        public GetActiveDrawingHandler(IDrawingRepository drawingRepository, IMapper mapper,
            IDistributedCacheService distributedCacheService)
        {
            _drawingRepository = drawingRepository;
            _mapper = mapper;
            _distributedCacheService = distributedCacheService;
        }

        public async Task<DrawingDTO> Handle(GetActiveDrawing query)
        {
            var activeDrawing = await _distributedCacheService.GetObjectOrDefaultAsync<DrawingDTO>(DistributedCacheService.ACTIVE_DRAWING);
            if (activeDrawing == null)
            {
                var drawing = await _drawingRepository.GetActiveAsync();
                activeDrawing = _mapper.Map<DrawingDTO>(drawing);
                await _distributedCacheService.SetAsync(DistributedCacheService.ACTIVE_DRAWING, activeDrawing);
            }

            return activeDrawing;
        }
    }
}
