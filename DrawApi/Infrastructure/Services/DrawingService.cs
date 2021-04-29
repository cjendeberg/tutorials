using System;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Services;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.ValueObjects;
using Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services
{
    public class DrawingService : IDrawingService
    {
        private readonly IDrawingRepository _drawingRepository;
        private readonly IDistributedCacheService _distributedCacheService;

        public DrawingService(IDrawingRepository drawingRepository, IDistributedCacheService distributedCacheService)
        {
            _drawingRepository = drawingRepository;
            _distributedCacheService = distributedCacheService;
        }

        public async Task<Drawing> CreateDrawing(DateTime startDate)
        {
            var activeDrawing = await _drawingRepository.GetActiveAsync();
            if (activeDrawing != null)
                throw new ServiceException($"Cannot create new drawing when other one is active (id: {activeDrawing.Id})!");

            var drawing = Drawing.CreateDrawing(startDate);
            await _drawingRepository.AddAsync(drawing);
            await _distributedCacheService.RemoveAsync(DistributedCacheService.ACTIVE_DRAWING);
            return drawing;
        }

        public async Task UpdateDrawingNumbers(Guid id, int[] numbers, int[] extraNumbers)
        {
            var drawing = await _drawingRepository.GetAsync(id);
            UpdateNumbers(drawing, numbers, extraNumbers);
            await _drawingRepository.UpdateAsync(drawing);
        }

        public async Task CompleteDrawing(Guid id, int[] numbers, int[] extraNumbers)
        {
            var drawing = await _drawingRepository.GetAsync(id);
            drawing.Deactivate();
            drawing.Complete();

            UpdateNumbers(drawing, numbers, extraNumbers);

            await _drawingRepository.UpdateAsync(drawing);
            await _distributedCacheService.RemoveAsync(DistributedCacheService.ACTIVE_DRAWING);
        }

        protected void UpdateNumbers(Drawing drawing, int[]numbers, int[]extraNumbers)
        {
            if (!drawing.HasNumbers())
            {
                foreach (var number in numbers)
                    drawing.AddNumber(Number.CreateNumber(number, false));
                foreach (var extraNumber in extraNumbers)
                    drawing.AddNumber(Number.CreateNumber(extraNumber, true));

                drawing.SetNumbersAddedAtDate();
            }
        }

        public Task<Drawing> GetDrawing(Guid id) => _drawingRepository.GetAsync(id);
    }
}
