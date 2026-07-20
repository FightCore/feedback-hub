using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fightcore.FeedbackHub.Models;
using Fightcore.FeedbackHub.Notifications.Discord;

namespace Fightcore.FeedbackHub.Notifications;

public class NotificationService
{
    private readonly HttpClient _httpClient;
    private readonly string _webhookUrl;

    public NotificationService(IConfiguration configuration)
    {
        var handler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        };
        _httpClient = new HttpClient(handler);
        _webhookUrl = configuration["WebhookUrl"];
        _httpClient.BaseAddress = new Uri(_webhookUrl);
    }
    
    public async Task Notify(FeedbackItem feedback)
    {
        var body = CreateWebhookBody(feedback);

        var result = await _httpClient.PostAsJsonAsync(_webhookUrl, body);
        if (!result.IsSuccessStatusCode)
        {
            throw new Exception(await result.Content.ReadAsStringAsync());
        }
    }

    private static WebhookBody CreateWebhookBody(FeedbackItem feedback)
    {
        // TODO strip down to allowed sizes
        return new WebhookBody()
        {
            Embeds =
            [
                new()
                {
                    Title = "Feedback received",
                    Timestamp = feedback.Timestamp.ToString("o", CultureInfo.InvariantCulture),
                    Fields =
                    [
                        new()
                        {
                            Name = "Description",
                            Value = feedback.Message
                        },
                        new()
                        {
                            Name = "Contact",
                            Value = feedback.ContactDetails
                        },
                        new EmbedField()
                        {
                            Name = "Source",
                            Value = feedback.Source
                        }
                    ]
                }
            ]
        };
    }
}