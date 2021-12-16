using System.Collections.Generic;
using System.Linq.Expressions;

namespace Me.One.Core.Utils
{
    public class ParameterHelper : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterHelper(
            Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(
            Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterHelper(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (map.TryGetValue(p, out var parameterExpression))
            {
                p = parameterExpression;
            }
            return base.VisitParameter(p);
        }
    }
}