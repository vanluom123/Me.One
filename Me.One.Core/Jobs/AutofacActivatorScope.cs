using System;
using Autofac;
using Hangfire;

namespace Me.One.Core.Jobs
{
    internal class AutofacActivatorScope : JobActivatorScope
    {
        internal ILifetimeScope _scope;

        public AutofacActivatorScope(IContainer container)
        {
            _scope = container.BeginLifetimeScope();
        }

        public override object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }

        public override void DisposeScope()
        {
            _scope.Dispose();
        }
    }
}