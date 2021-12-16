using System;
using System.Collections.Generic;
using Hangfire;
using Hangfire.Annotations;

namespace Me.One.Core.Jobs
{
    internal class SimpleJobActivatorScope : JobActivatorScope
    {
        private readonly JobActivator _activator;
        private readonly List<IDisposable> _disposables = new();

        public SimpleJobActivatorScope([NotNull] JobActivator activator)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        public override object Resolve(Type type)
        {
            var obj = _activator.ActivateJob(type);
            if (obj is not IDisposable disposable)
                return obj;
            _disposables.Add(disposable);
            return obj;
        }

        public override void DisposeScope()
        {
            if (_activator is HangfireActivatorCore activator)
                activator._scope.Dispose();
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}