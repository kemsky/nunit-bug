using System.Linq.Expressions;

namespace common.Expressions;

public class ReplaceParameterVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    private readonly Expression _replacement;

    public ReplaceParameterVisitor(ParameterExpression parameter, Expression replacement)
    {
        _parameter = parameter;
        _replacement = replacement;
    }

    protected override Expression VisitParameter(ParameterExpression parameterExpression)
    {
        if (parameterExpression == _parameter)
        {
            return _replacement;
        }

        return base.Visit(parameterExpression);
    }
}