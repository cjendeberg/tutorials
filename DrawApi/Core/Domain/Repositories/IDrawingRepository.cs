using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Entities;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Repositories
{
    public interface IDrawingRepository : IRepository
    {
        Task<Drawing> GetAsync(Guid id);
        Task AddAsync(Drawing drawing);
        Task UpdateAsync(Drawing drawing);
        Task<Drawing> GetActiveAsync();
    }
}
