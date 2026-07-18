using Fightcore.FeedbackHub.Models;

namespace Fightcore.FeedbackHub.Notifications;

public class NotificationService
{
    public Task Notify(FeedbackItem feedback)
    {
        return Task.CompletedTask;
    }
}