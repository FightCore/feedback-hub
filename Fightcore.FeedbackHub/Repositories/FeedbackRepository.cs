using Dapper;
using Fightcore.FeedbackHub.Models;
using Microsoft.Data.Sqlite;

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
        var connection = new SqliteConnection(_configuration.GetConnectionString("FeedbackHub"));
        await connection.OpenAsync();
        const string createTableSql =
            "CREATE TABLE IF NOT EXISTS feedback (id INTEGER PRIMARY KEY AUTOINCREMENT, message TEXT, contact_details TEXT, timestamp TEXT, source TEXT)";
        
        await connection.ExecuteAsync(createTableSql);
        const string insertSql = "INSERT INTO feedback (message, contact_details, timestamp, source) VALUES (@Message, @ContactDetails, @Timestamp, @Source)";
        await connection.ExecuteAsync(insertSql, feedback);
    }
}