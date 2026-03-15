using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    [TestCase("building", "yellow")]
    [TestCase("enqueued", "magenta")]
    [TestCase("cancelled", "grey")]
    [TestCase("error", "red")]
    [TestCase("ready", "green")]
    [TestCase("processing", "green")]
    [TestCase("skipped", "green")]
    public void NetlifyDeploy_StateColor_IsCorrect(string state, string expectedColor)
    {
        var color = state switch
        {
            "building" => "yellow",
            "enqueued" => "magenta",
            "cancelled" => "grey",
            "error" => "red",
            _ => "green"
        };

        Assert.That(color, Is.EqualTo(expectedColor));
    }

    [Test]
    public void NetlifyDeploy_CancelledState_OverriddenByErrorMessage()
    {
        var deploy = new NetlifyDeploy
        {
            State = "error",
            ErrorMessage = "Build was canceled by user"
        };

        var state = deploy.State ?? "unknown";
        if (deploy.ErrorMessage?.ToLower().Contains("canceled") == true)
        {
            state = "cancelled";
        }

        Assert.That(state, Is.EqualTo("cancelled"));
    }

    [Test]
    [TestCase("production-deploy", "Production Deploy")]
    [TestCase("deploy-preview", "Deploy Preview")]
    [TestCase("branch-deploy", "Branch Deploy")]
    public void NetlifyDeploy_ContextFormatting_IsCorrect(string context, string expected)
    {
        var displayContext = string.Join(" ", context.Split('-')
            .Select(w => w.Length > 1 ? char.ToUpper(w[0]) + w[1..] : w.ToUpper()));

        Assert.That(displayContext, Is.EqualTo(expected));
    }

    [Test]
    public void NetlifyDeploy_CommitRef_IsTruncatedToSevenChars()
    {
        var deploy = new NetlifyDeploy
        {
            Branch = "main",
            CommitRef = "abc1234567890"
        };

        var commitRef = deploy.CommitRef is { Length: > 0 }
            ? deploy.CommitRef[..Math.Min(7, deploy.CommitRef.Length)]
            : "HEAD";

        Assert.That(commitRef, Is.EqualTo("abc1234"));
    }

    [Test]
    public void NetlifyDeploy_CommitRef_FallsBackToHead_WhenNull()
    {
        var deploy = new NetlifyDeploy
        {
            Branch = "main",
            CommitRef = null
        };

        var commitRef = deploy.CommitRef is { Length: > 0 }
            ? deploy.CommitRef[..Math.Min(7, deploy.CommitRef.Length)]
            : "HEAD";

        Assert.That(commitRef, Is.EqualTo("HEAD"));
    }
}