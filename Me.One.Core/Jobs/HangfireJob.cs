using System;
using System.Linq.Expressions;
using Hangfire;

namespace Me.One.Core.Jobs
{
    public class HangfireJob : IBackgroundJob
    {
        public string Enqueue(Expression<Action> action, TimeSpan? time = null)
        {
            return time.HasValue ? BackgroundJob.Schedule(action, time.Value) : BackgroundJob.Enqueue(action);
        }

        public string Enqueue(Expression<Action> action, DateTimeOffset? time = null)
        {
            return time.HasValue ? BackgroundJob.Schedule(action, time.Value) : BackgroundJob.Enqueue(action);
        }

        public string Enqueue<T>(Expression<Action<T>> action, TimeSpan? time = null)
        {
            return time.HasValue ? BackgroundJob.Schedule(action, time.Value) : BackgroundJob.Enqueue(action);
        }

        public string Enqueue<T>(Expression<Action<T>> action, DateTimeOffset? time = null)
        {
            return time.HasValue ? BackgroundJob.Schedule(action, time.Value) : BackgroundJob.Enqueue(action);
        }

        public void Recurring(
            string id,
            Expression<Action> action,
            string cron,
            TimeZoneInfo timeZone = null,
            string queue = "default")
        {
            RecurringJob.AddOrUpdate(id, action, cron, timeZone, queue);
        }

        public void Recurring<T>(
            string id,
            Expression<Action<T>> action,
            string cron,
            TimeZoneInfo timeZone = null,
            string queue = "default")
        {
            RecurringJob.AddOrUpdate(id, action, cron, timeZone, queue);
        }

        public void RemovJob(string id)
        {
            RecurringJob.RemoveIfExists(id);
        }

        public void Trigger(string id)
        {
            RecurringJob.Trigger(id);
        }
    }
}