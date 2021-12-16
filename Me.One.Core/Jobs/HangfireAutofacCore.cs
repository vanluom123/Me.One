using Autofac;
using Hangfire;
using Hangfire.Server;

namespace Me.One.Core.Jobs
{
    public class HangfireAutofacCore : JobActivator
    {
        private readonly IContainer _container;

        public HangfireAutofacCore(IContainer container)
        {
            _container = container;
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new AutofacActivatorScope(_container);
        }

        public override JobActivatorScope BeginScope(PerformContext context)
        {
            return BeginScope(new JobActivatorContext(context.Connection, context.BackgroundJob,
                context.CancellationToken));
        }
    }
}