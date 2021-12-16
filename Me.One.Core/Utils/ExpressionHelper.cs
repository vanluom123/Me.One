using System;
using System.Linq;
using System.Linq.Expressions;

namespace Me.One.Core.Utils
{
    public static class ExpressionHelper
    {
        private static Expression<T> Compose<T>(
            this Expression<T> first,
            Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            if (first == null && second == null)
                return null;
            if (first == null)
                return second;
            if (second == null)
                return first;
            var expression = ParameterHelper.ReplaceParameters(first.Parameters.Select((f, i) => new
            {
                f,
                s = second.Parameters[i]
            }).ToDictionary(p => p.s, p => p.f), second.Body);
            return Expression.Lambda<T>(merge(first.Body, expression), first.Parameters);
        }

        public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>> target)
        {
            return target;
        }

        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> target,
            params Expression<Func<T, bool>>[] expressions)
        {
            var seed = target;
            if (target != null)
                seed = expressions.Aggregate(seed,
                    (Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>>) ((current,
                        t) => current.Compose(t, Expression.AndAlso)));
            return seed;
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> target,
            params Expression<Func<T, bool>>[] expressions)
        {
            var seed = target;
            if (target != null)
                seed = expressions.Aggregate(seed,
                    (Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>>) ((current,
                        t) => current.Compose(t, Expression.OrElse)));
            return seed;
        }

        public static string GetMemberName<T>(this Expression<T> expression)
        {
            if (expression?.Body is MemberExpression body)
                return body.Member.Name;
            throw new ArgumentException(
                $"The argument of type {expression?.GetType().ToString() ?? "null"} is invalid.");
        }
    }
}