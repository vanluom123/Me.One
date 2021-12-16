using System;
using System.Linq.Expressions;

namespace Me.One.Core.Jobs
{
    public interface IBackgroundJob
    {
        string Enqueue(Expression<Action> action, TimeSpan? time = null);

        string Enqueue(Expression<Action> action, DateTimeOffset? time = null);

        string Enqueue<T>(Expression<Action<T>> action, TimeSpan? time = null);

        string Enqueue<T>(Expression<Action<T>> action, DateTimeOffset? time = null);

        void Recurring(
            string id,
            Expression<Action> action,
            string cron,
            TimeZoneInfo timeZone = null,
            string queue = "default");

        void Recurring<T>(
            string id,
            Expression<Action<T>> action,
            string cron,
            TimeZoneInfo timeZone = null,
            string queue = "default");

        void RemovJob(string id);

        void Trigger(string id);
    }
}