using System;
using System.Threading.Tasks;

namespace Me.One.Core.Redis
{
    public interface IRedisService
    {
        Task TryConnect(int retryTime);

        Task Connect();

        Task SetAsync(string key, string value, TimeSpan? expiry);

        Task SetAsync<T>(string key, T value, TimeSpan? expiry);

        Task<T> GetAsync<T>(string key);

        Task DeleteAsync(string key);
    }
}