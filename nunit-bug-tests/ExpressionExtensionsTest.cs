using System.Linq.Expressions;
using common.Expressions;
using NUnit.Framework;

namespace nunit_bug_tests;

[Parallelizable(ParallelScope.All)]
[TestFixture]
public class ExpressionExtensionsTest
{
    private string String { get; } = "String";

    private bool Bool { get; } = true;

    private BindArgument Complex { get; } = new BindArgument();

    #region TryEvaluate

    [Test]
    public void TryEvaluate__constant__ok()
    {
        Expression<Func<string>> subject = () => "constant";

        var success = subject.Body.TryEvaluate(out var value);

        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("constant"));
    }

    [Test]
    public void TryEvaluate__class_static_field__ok()
    {
        Expression<Func<string>> subject = () => TryEvaluate.StringStaticField;

        var success = subject.Body.TryEvaluate(out var value);

        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("constant"));
    }

    [Test]
    public void TryEvaluate__class_static_property__ok()
    {
        Expression<Func<string>> subject = () => TryEvaluate.StringStaticProperty;

        var success = subject.Body.TryEvaluate(out var value);

        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("constant"));
    }

    [Test]
    public void TryEvaluate__unsupported_call__expression()
    {
        Expression<Func<string>> subject = () => TryEvaluate.StringStaticFunc();

        var success = subject.Body.TryEvaluate(out _);

        Assert.That(success, Is.False);
    }

    [Test]
    public void TryEvaluate__unsupported_static__expression()
    {
        Expression<Func<Func<string>>> subject = () => TryEvaluate.StringStaticFunc;

        var success = subject.Body.TryEvaluate(out _);

        Assert.That(success, Is.False);
    }

    [Test]
    public void TryEvaluate__unsupported_param__expression()
    {
        Expression<Func<string, string>> subject = x => x;

        var success = subject.Body.TryEvaluate(out _);

        Assert.That(success, Is.False);
    }

    [Test]
    public void TryEvaluate__unsupported_param_prop__expression()
    {
        Expression<Func<TryEvaluate, string>> subject = x => x.StringField;

        var success = subject.Body.TryEvaluate(out _);

        Assert.That(success, Is.False);
    }

    [Test]
    public void TryEvaluate__class_field__ok()
    {
        var instance = new TryEvaluate();

        Expression<Func<string>> subject = () => instance.StringField;

        var success = subject.Body.TryEvaluate(out var value);

        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("constant"));
    }

    [Test]
    public void TryEvaluate__class_property__ok()
    {
        var instance = new TryEvaluate();

        Expression<Func<string>> subject = () => instance.StringProperty;

        var success = subject.Body.TryEvaluate(out var value);

        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("constant"));
    }

    [Test]
    public void TryEvaluate__local_var__ok()
    {
        var local = "constant";

        Expression<Func<string>> subject = () => local;

        var success = subject.Body.TryEvaluate(out var value);

        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("constant"));
    }

    [Test]
    [TestCase("constant")]
    public void TryEvaluate__local_parameter__ok(string parameter)
    {
        Expression<Func<string>> subject = () => parameter;

        var success = subject.Body.TryEvaluate(out var value);

        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("constant"));
    }

    #endregion

    #region Compose

    [Test]
    public void And__ok()
    {
        Expression<Func<string, bool>> expression1 = x => x == "expression1";
        Expression<Func<string, bool>> expression2 = y => y == "expression2";

        Expression<Func<string, bool>> result = expression1.And(expression2);

        Assert.That(result.ToString(), Is.EqualTo("x => ((x == \"expression1\") AndAlso (x == \"expression2\"))"));
    }

    [Test]
    public void Or__ok()
    {
        Expression<Func<string, bool>> expression1 = x => x == "expression1";
        Expression<Func<string, bool>> expression2 = y => y == "expression2";

        Expression<Func<string, bool>> result = expression1.Or(expression2);

        Assert.That(result.ToString(), Is.EqualTo("x => ((x == \"expression1\") OrElse (x == \"expression2\"))"));
    }

    #endregion

    [Test]
    public void Not__ok()
    {
        Expression<Func<string, bool>> expression = x => x == "expression";

        Expression<Func<string, bool>> result = expression.Not();

        Assert.That(result.ToString(), Is.EqualTo("x => Not((x == \"expression\"))"));
    }

    [Test]
    public void ApplyTo__ok()
    {
        Expression<Func<string, bool>> expression = x => x == "1";
        Expression<Func<long, string>> source = y => y.ToString();

        Expression<Func<long, bool>> result = expression.ApplyTo(source);

        Assert.That(result.ToString(), Is.EqualTo("y => (y.ToString() == \"1\")"));
    }

    [Test]
    public void To__ok()
    {
        Expression<Func<long, string>> expression = x => x.ToString();
        Expression<Func<string, bool>> projection = y => y == "1";

        Expression<Func<long, bool>> result = expression.ProjectTo(projection);

        Assert.That(result.ToString(), Is.EqualTo("x => (x.ToString() == \"1\")"));
    }


    private class BindArgument
    {
        // ReSharper disable once InconsistentNaming
        public string x = "x";
    }

    private class TryEvaluate
    {
        public static readonly string StringStaticField = "constant";

        public static string StringStaticProperty { get; } = "constant";

        public static string StringStaticFunc() => "constant";

        // ReSharper disable once InconsistentNaming
        public readonly string StringField = "constant";

        public string StringProperty { get; } = "constant";
    }
}