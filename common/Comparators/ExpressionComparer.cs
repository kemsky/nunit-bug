using System.Linq.Expressions;

namespace common.Comparators;

/// <summary>
/// Expressions are compared as Strings, so x => x is not the same as y => y.
/// </summary>
/// <typeparam name="T">Expression type</typeparam>
public sealed class ExpressionComparer<T> : IEqualityComparer<T> where T : LambdaExpression
{
    public bool Equals(T x, T y)
    {
        if ((x == null && y == null) || ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.ToString() == y.ToString();
    }

    public int GetHashCode(T obj)
    {
        return obj == null ? 0 : obj.ToString().GetHashCode(StringComparison.Ordinal);
    }
}