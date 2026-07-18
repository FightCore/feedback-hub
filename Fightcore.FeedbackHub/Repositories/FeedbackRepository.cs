using Dapper;
using Fightcore.FeedbackHub.Models;
using Npgsql;

namespace Fightcore.FeedbackHub.Repositories;

public class FeedbackRepository
{
    private readonly IConfiguration _configuration;
    
    public FeedbackRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Ingest(FeedbackItem feedback)
    {
        var connection = new NpgsqlConnection(_configuration.GetConnectionString("FeedbackHub"));
        await connection.OpenAsync();
        const string insertSql = "INSERT INTO feedback (message, contact_details, timestamp, source) VALUES (@Message, @ContactDetails, @Timestamp, @Source)";
        await connection.ExecuteAsync(insertSql, feedback);
    }
}