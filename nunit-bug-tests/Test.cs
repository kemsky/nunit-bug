using NUnit.Framework;

namespace nunit_bug_tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Test
{
    [Test]
    public void Test__SqlClientFactory()
    {
        Assert.That(Microsoft.Data.SqlClient.SqlClientFactory.Instance, Is.Not.Null);
    }
}