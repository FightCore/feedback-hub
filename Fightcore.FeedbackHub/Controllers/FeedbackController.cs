using Fightcore.FeedbackHub.Models;
using Fightcore.FeedbackHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fightcore.FeedbackHub.Controllers;

public class FeedbackController : ControllerBase
{
    private readonly FeedbackService _feedbackService;

    public FeedbackController(FeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost("feedback-items")]
    public async Task<IActionResult> Create([FromBody] FeedbackItem feedbackItem)
    {
        feedbackItem.Timestamp = DateTime.UtcNow;
        
        await _feedbackService.Process(feedbackItem);
        return Ok();
    }
}