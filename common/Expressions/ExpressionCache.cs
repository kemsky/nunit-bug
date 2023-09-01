using System.Collections.Concurrent;
using System.Linq.Expressions;
using common.Comparators;

namespace common.Expressions;

public static class ExpressionCache<T, TResult>
{
    private static readonly ConcurrentDictionary<Expression<Func<T, TResult>>, Func<T, TResult>> Cache = new ConcurrentDictionary<Expression<Func<T, TResult>>, Func<T, TResult>>(new ExpressionComparer<Expression<Func<T, TResult>>>());

    public static Func<T, TResult> CachedCompile(Expression<Func<T, TResult>> targetSelector)
    {
        return Cache.GetOrAdd(targetSelector, _ => targetSelector.Compile()); // todo: #2803
    }
}

public static class ExpressionCache<T>
{
    private static readonly ConcurrentDictionary<Expression<Action<T>>, Action<T>> Cache = new ConcurrentDictionary<Expression<Action<T>>, Action<T>>(new ExpressionComparer<Expression<Action<T>>>());

    public static Action<T> CachedCompile(Expression<Action<T>> targetSelector)
    {
        return Cache.GetOrAdd(targetSelector, _ => targetSelector.Compile()); // todo: #2803
    }
}