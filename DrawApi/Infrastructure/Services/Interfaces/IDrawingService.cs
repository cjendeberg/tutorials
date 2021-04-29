using System;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Common.Services;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities;

namespace Zero99Lotto.SRC.Services.Draws.API.Infrastructure.Services.Interfaces
{
    public interface IDrawingService : IService
    {
        Task<Drawing> CreateDrawing(DateTime startTime);
        Task CompleteDrawing(Guid id, int[] numbers, int[] extraNumbers);
        Task UpdateDrawingNumbers(Guid id, int[] numbers, int[] extraNumbers);
        Task<Drawing> GetDrawing(Guid id);
    }
}
