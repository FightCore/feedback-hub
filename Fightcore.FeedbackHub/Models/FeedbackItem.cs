namespace Fightcore.FeedbackHub.Models;

/// <summary>
/// A feedback item that a user has submitted.
/// </summary>
public class FeedbackItem
{
    /// <summary>
    /// The feedback message they have left.
    /// </summary>
    public required string Message { get; set; }
    
    /// <summary>
    /// The optional details on how we can contact them.
    /// </summary>
    public string ContactDetails { get; set; }
    
    /// <summary>
    /// The timestamp when this feedback item was created.
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// The source where the feedback came from.
    /// </summary>
    public required string Source { get; set; }
}