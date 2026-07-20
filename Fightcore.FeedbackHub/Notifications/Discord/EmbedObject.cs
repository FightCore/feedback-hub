namespace Fightcore.FeedbackHub.Notifications.Discord;

public class EmbedObject
{
    public required string Title { get; set; }

    public string Type { get; set; } = "rich";
    
    public string Description { get; set; }
    
    public string Timestamp { get; set; }
    
    public EmbedField[] Fields { get; set; }
}