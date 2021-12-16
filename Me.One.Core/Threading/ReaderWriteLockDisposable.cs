using System;
using System.Threading;

namespace Me.One.Core.Threading
{
    public class ReaderWriteLockDisposable : IDisposable
    {
        private readonly ReaderWriteLockType _readerWriteLockType;
        private readonly ReaderWriterLockSlim _rwLock;
        private bool _disposed;

        public ReaderWriteLockDisposable(
            ReaderWriterLockSlim rwLock,
            ReaderWriteLockType readerWriteLockType = ReaderWriteLockType.Write)
        {
            _rwLock = rwLock;
            _readerWriteLockType = readerWriteLockType;
            switch (_readerWriteLockType)
            {
                case ReaderWriteLockType.Read:
                    _rwLock.EnterReadLock();
                    break;
                case ReaderWriteLockType.Write:
                    _rwLock.EnterWriteLock();
                    break;
                case ReaderWriteLockType.UpgradeableRead:
                    _rwLock.EnterUpgradeableReadLock();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                switch (_readerWriteLockType)
                {
                    case ReaderWriteLockType.Read:
                        _rwLock.ExitReadLock();
                        break;
                    case ReaderWriteLockType.Write:
                        _rwLock.ExitWriteLock();
                        break;
                    case ReaderWriteLockType.UpgradeableRead:
                        _rwLock.ExitUpgradeableReadLock();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _disposed = true;
        }
    }
}