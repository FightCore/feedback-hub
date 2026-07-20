using Fightcore.FeedbackHub.Models;
using Fightcore.FeedbackHub.Notifications;
using Fightcore.FeedbackHub.Repositories;
using Polly;
using Polly.Retry;

namespace Fightcore.FeedbackHub.Services;

public class FeedbackService
{
    private readonly FeedbackRepository _feedbackRepository;
    private readonly NotificationService _notificationService;
    private readonly HashSet<string> _acceptedSources = ["fightcore-bot", "fightcore-web"];
    private readonly ILogger<FeedbackService> _logger;
    
    public FeedbackService(FeedbackRepository feedbackRepository, NotificationService notificationService, ILogger<FeedbackService> logger)
    {
        _feedbackRepository = feedbackRepository;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Process(FeedbackItem feedback)
    {
        if (feedback == null)
        {
            return;
        }

        // If the accepted sources does not contain the provided source, we silently fail the request.
        // Prevents some possible spam.
        if (!_acceptedSources.Contains(feedback.Source))
        {
            return;
        }
        
        // Default pipeline with 3 retries every 10 seconds.
        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions())
            .AddTimeout(TimeSpan.FromSeconds(10))
            .Build();
        
        _logger.LogInformation("Processing feedback {feedback}", feedback.Message);

        await pipeline.ExecuteAsync(async token =>
        {
            await _feedbackRepository.Ingest(feedback);
        });
        
        await pipeline.ExecuteAsync(async token =>
        {
            await _notificationService.Notify(feedback);
        });
    }
}