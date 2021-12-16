using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Me.One.Core.Redis
{
    public class RedisService : IRedisService
    {
        private readonly string _host;
        private readonly string _password;
        private readonly int _port;
        private ConnectionMultiplexer _redis;

        public RedisService(string host, string password = null, int port = 6379)
        {
            _host = host;
            if (!string.IsNullOrEmpty(password))
                _password = ",password=" + password;
            _port = port;
            TryConnect().Wait();
        }

        public async Task TryConnect(int retryTime = 5)
        {
            try
            {
                if (_redis != null)
                    return;
                _redis = await ConnectionMultiplexer.ConnectAsync(string.Format("{0}:{1}{2},connectRetry={3}", _host,
                    _port, _password, retryTime));
            }
            catch (RedisConnectionException ex)
            {
                throw ex;
            }
        }

        public async Task Connect()
        {
            try
            {
                if (_redis != null)
                    return;
                _redis = await ConnectionMultiplexer.ConnectAsync(string.Format("{0}:{1}{2}", _host, _port, _password));
            }
            catch (RedisConnectionException ex)
            {
                throw ex;
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;
            await TryConnect();
            return JsonConvert.DeserializeObject<T>(await _redis.GetDatabase()
                .StringGetAsync((RedisKey) key.ToUpper()));
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key))
                return;
            await TryConnect();
            var database = _redis.GetDatabase();
            if (!expiry.HasValue)
                expiry = new TimeSpan(1, 0, 0, 0);
            var upper = (RedisKey) key.ToUpper();
            var redisValue = (RedisValue) value;
            var nullable = expiry;
            var num = await database.StringSetAsync(upper, redisValue, nullable) ? 1 : 0;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key))
                return;
            await TryConnect();
            var database = _redis.GetDatabase();
            if (!expiry.HasValue)
                expiry = new TimeSpan(1, 0, 0, 0);
            var upper = (RedisKey) key.ToUpper();
            var redisValue = (RedisValue) JsonConvert.SerializeObject(value);
            var nullable = expiry;
            var num = await database.StringSetAsync(upper, redisValue, nullable) ? 1 : 0;
        }

        public async Task DeleteAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;
            await TryConnect();
            _redis.GetDatabase().KeyDelete((RedisKey) key.ToUpper());
        }
    }
}