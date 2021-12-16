using System;

namespace Me.One.Core.Caching
{
    public interface ICacheLocker
    {
        bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
    }
}