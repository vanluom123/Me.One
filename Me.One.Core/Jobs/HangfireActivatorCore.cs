using System;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Me.One.Core.Jobs
{
    public class HangfireActivatorCore : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;
        internal IServiceScope _scope;

        public HangfireActivatorCore(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            _scope = _serviceProvider.CreateScope();
            return _scope.ServiceProvider.GetService(jobType);
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new SimpleJobActivatorScope(this);
        }

        public override JobActivatorScope BeginScope(PerformContext context)
        {
            return BeginScope(new JobActivatorContext(context.Connection, context.BackgroundJob,
                context.CancellationToken));
        }
    }
}