using System.Linq.Expressions;
using common.Expressions;
using NUnit.Framework;

namespace nunit_bug_tests;

[Parallelizable(ParallelScope.All)]
[TestFixture]
public class ExpressionExtensionsTest
{
    [Test]
    public void TryEvaluate__constant__ok()
    {
        Expression<Func<string>> subject = () => "constant";

        var success = subject.Body.TryEvaluate(out var value);

        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("constant"));
    }
}