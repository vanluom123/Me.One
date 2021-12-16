using System;
using System.Net;
using StackExchange.Redis;

namespace Me.One.Core.Caching.Redis
{
    public interface IRedisConnectionWrapper : IDisposable
    {
        IDatabase GetDatabase(int db);

        IServer GetServer(EndPoint endPoint);

        EndPoint[] GetEndPoints();

        void FlushDatabase(RedisDatabaseNumber db);
    }
}