using System;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Common.Services
{
    public interface IDataInitializer : IService
    {
        Task SeedAsync(IServiceProvider serviceProvider);
    }
}
