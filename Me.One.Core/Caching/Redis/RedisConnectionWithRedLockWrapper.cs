using System;
using System.Linq;
using System.Net;
using Me.One.Core.Configuration;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Me.One.Core.Caching.Redis
{
    public class RedisConnectionWithRedLockWrapper : IRedisConnectionWrapper, ICacheLocker
    {
        private readonly AppStartupConfig _config;
        private readonly Lazy<string> _connectionString;
        private readonly object _lock = new();
        private volatile ConnectionMultiplexer _connection;
        private bool _disposed;
        private volatile RedLockFactory _redisLockFactory;

        public RedisConnectionWithRedLockWrapper(AppStartupConfig config)
        {
            _config = config;
            _connectionString = new Lazy<string>(GetConnectionString);
            _redisLockFactory = CreateRedisLockFactory();
        }

        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            using var redLock = _redisLockFactory.CreateLock(resource, expirationTime);
            if (!redLock.IsAcquired)
            {
                return false;
            }
            action();
            return true;
        }

        public IDatabase GetDatabase(int db)
        {
            return GetConnection().GetDatabase(db);
        }

        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        public void FlushDatabase(RedisDatabaseNumber db)
        {
            foreach (var endPoint in GetEndPoints())
            {
                GetServer(endPoint).FlushDatabase((int) db);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected string GetConnectionString()
        {
            return _config.RedisConnectionString;
        }

        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection;
            }
            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected)
                {
                    return _connection;
                }
                _connection?.Dispose();
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        protected RedLockFactory CreateRedisLockFactory()
        {
            var configurationOptions = ConfigurationOptions.Parse(_connectionString.Value);
            return RedLockFactory.Create(GetEndPoints().Select((Func<EndPoint, RedLockEndPoint>) (endPoint =>
                new RedLockEndPoint
                {
                    EndPoint = endPoint,
                    Password = configurationOptions.Password,
                    Ssl = configurationOptions.Ssl,
                    RedisDatabase = configurationOptions.DefaultDatabase,
                    ConfigCheckSeconds = configurationOptions.ConfigCheckSeconds,
                    ConnectionTimeout = configurationOptions.ConnectTimeout,
                    SyncTimeout = configurationOptions.SyncTimeout
                })).ToList());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _connection?.Dispose();
                _redisLockFactory?.Dispose();
            }

            _disposed = true;
        }
    }
}