using Terminal.Dashboard.Panes.Helpers;

namespace Terminal.Dashboard.Panes;

public class ShortcutTests
{
    [Test]
    public void Formatter_Story_FormatsCorrectly()
    {
        var story = new ShortcutStory
        {
            Id = 1234,
            Name = "Fix the login bug",
            WorkflowState = new ShortcutWorkflowState { Id = 1, Name = "In Progress" },
            Owners = new List<ShortcutMember>
            {
                new ShortcutMember
                {
                    Id = "abc-123",
                    Profile = new ShortcutMemberProfile { MentionName = "jdoe", Name = "Jane Doe" }
                }
            }
        };

        var result = ShortcutHelper.Formatter.Story(story);

        Assert.That(result, Does.Contain("sc-1234"));
        Assert.That(result, Does.Contain("In Progress"));
        Assert.That(result, Does.Contain("jdoe"));
        Assert.That(result, Does.Contain("Fix the login bug"));
    }

    [Test]
    public void Formatter_Story_HandlesNoOwners()
    {
        var story = new ShortcutStory
        {
            Id = 5678,
            Name = "Unassigned story",
            WorkflowState = new ShortcutWorkflowState { Id = 2, Name = "Ready for Development" },
            Owners = new List<ShortcutMember>()
        };

        var result = ShortcutHelper.Formatter.Story(story);

        Assert.That(result, Does.Contain("sc-5678"));
        Assert.That(result, Does.Contain("Ready for Development"));
        Assert.That(result, Does.Contain("Unassigned story"));
    }

    [Test]
    public void Formatter_Story_HandlesNullWorkflowState()
    {
        var story = new ShortcutStory
        {
            Id = 9999,
            Name = "Story without state",
            WorkflowState = null,
            Owners = new List<ShortcutMember>()
        };

        var result = ShortcutHelper.Formatter.Story(story);

        Assert.That(result, Does.Contain("sc-9999"));
        Assert.That(result, Does.Contain("Unknown"));
        Assert.That(result, Does.Contain("Story without state"));
    }

    [Test]
    public void Formatter_Story_HandlesMultipleOwners()
    {
        var story = new ShortcutStory
        {
            Id = 1111,
            Name = "Collaborative story",
            WorkflowState = new ShortcutWorkflowState { Id = 3, Name = "In Review" },
            Owners = new List<ShortcutMember>
            {
                new ShortcutMember
                {
                    Id = "abc-001",
                    Profile = new ShortcutMemberProfile { MentionName = "alice", Name = "Alice" }
                },
                new ShortcutMember
                {
                    Id = "abc-002",
                    Profile = new ShortcutMemberProfile { MentionName = "bob", Name = "Bob" }
                }
            }
        };

        var result = ShortcutHelper.Formatter.Story(story);

        Assert.That(result, Does.Contain("alice"));
        Assert.That(result, Does.Contain("bob"));
    }
}
