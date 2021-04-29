using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Common.Services
{
    public class DistributedCacheService : IDistributedCacheService
    {
        public const string GAME_TYPES = "Game_Types";
        public const string IS_SHUTDOWN = "Is_Shutdown";
        public const string PRIZES = "Prizes";
        public const string COUNTRIES_PHONE_CODES = "Countries_Phone_Codes";
        public const string ACTIVE_DRAWING = "Active_Drawing";

        private readonly IDistributedCache _distributedCache;

        public DistributedCacheService(IDistributedCache distributedCache)
            => _distributedCache = distributedCache;

        public async Task<T> GetObjectOrDefaultAsync<T>(string key)
        {
            var typesFromCache = await _distributedCache.GetAsync(key);
            if (typesFromCache != null)
                return JsonConvert.DeserializeObject<T>(System.Text.Encoding.UTF8.GetString(typesFromCache));

            return default;
        }

        public Task RemoveAsync(string key)
            => _distributedCache.RemoveAsync(key);

        /// <summary>
        /// Adds or updates
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task SetAsync(string key, byte[] value)
            => _distributedCache.SetAsync(key, value);

        public Task SetAsync(string key, string value)
            => _distributedCache.SetStringAsync(key, value);

        public Task SetAsync(string key, object value)
            => SetAsync(key, JsonConvert.SerializeObject(value));
    }
}
