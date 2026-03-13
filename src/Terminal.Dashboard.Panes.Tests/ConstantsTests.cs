namespace Terminal.Dashboard.Panes;

public class ConstantsTests
{
    [Test]
    public void GitHubClient_ProductHeaderValue_IsNotNull()
    {
        Assert.That(Constants.GitHubClient_ProductHeaderValue, Is.Not.Null);
    }

    [Test]
    public void GitHubClient_ProductHeaderValue_IsExpectedValue()
    {
        Assert.That(Constants.GitHubClient_ProductHeaderValue, Is.EqualTo("Terminal.Dashboard"));
    }
}
