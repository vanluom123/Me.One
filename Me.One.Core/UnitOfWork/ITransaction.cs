using System;

namespace Me.One.Core.UnitOfWork
{
    public interface ITransaction : IDisposable
    {
        void Commit();
    }
}