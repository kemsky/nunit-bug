using System.Linq.Expressions;
using System.Reflection;

namespace common.Expressions;

public static class ExpressionExtensions
{
    public static bool TryEvaluate(this Expression expression, out object result)
    {
        if (expression is MemberExpression memberExpression)
        {
            if (memberExpression.Expression == null)
            {
                if (memberExpression.Member is PropertyInfo staticProperty)
                {
                    result = staticProperty.GetValue(null);

                    return true;
                }
                else if (memberExpression.Member is FieldInfo staticField)
                {
                    result = staticField.GetValue(null);

                    return true;
                }
            }

            var stack = new Stack<MemberExpression>();

            var e = memberExpression;

            while (e != null)
            {
                stack.Push(e);

                e = e.Expression as MemberExpression;
            }

            if (stack.Peek().Expression is ConstantExpression constantExpression)
            {
                var value = constantExpression.Value;

                while (stack.TryPop(out var stackExpression))
                {
                    if (stackExpression.Member is PropertyInfo propertyInfo)
                    {
                        value = propertyInfo.GetValue(value);
                    }
                    else if (stackExpression.Member is FieldInfo fieldInfo)
                    {
                        value = fieldInfo.GetValue(value);
                    }
                }

                result = value;

                return true;
            }
            else
            {
                result = null;

                return false;
            }
        }
        else if (expression is ConstantExpression constantExpression)
        {
            result = constantExpression.Value;

            return true;
        }

        result = null;

        return false;
    }
}