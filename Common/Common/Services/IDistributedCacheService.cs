using System.Net.Http;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Common.Services
{
    public interface IDistributedCacheService : IService
    {
        Task<T> GetObjectOrDefaultAsync<T>(string key);
        Task SetAsync(string key, byte[] value);
        Task SetAsync(string key, object value);
        Task SetAsync(string key, string value);
        Task RemoveAsync(string key);
    }
}
