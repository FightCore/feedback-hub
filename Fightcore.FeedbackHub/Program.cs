using Fightcore.FeedbackHub.Notifications;
using Fightcore.FeedbackHub.Repositories;
using Fightcore.FeedbackHub.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>()
    .AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<FeedbackRepository>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<FeedbackService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    options.AddPolicy("AllowFightcore",
        policy => policy
            .WithOrigins(["https://www.fightcore.gg", "https://beta.fightcore.gg"])
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("AllowFightcore");
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();