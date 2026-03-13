namespace Terminal.Dashboard.Panes;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Constants_IsStaticClass()
    {
        var type = typeof(Constants);
        Assert.That(type.IsAbstract && type.IsSealed, Is.True);
    }
}