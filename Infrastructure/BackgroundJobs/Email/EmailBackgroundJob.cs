using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Application.DTOs.Email;
using Application.Services;
using Infrastructure.Implementations.Persistence.Contexts;

public class EmailBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailBackgroundService> _logger;

    public EmailBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        IEmailService emailService,
        ILogger<EmailBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _emailService = emailService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Background Service is running...");

        //while (!stoppingToken.IsCancellationRequested)
        //{
        //    try
        //    {
        //        using (var scope = _serviceScopeFactory.CreateScope()) 
        //        {
        //            var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

        //            var users = await dbContext.ExecuteRawSqlQueryListAsync<UserEmailDto>(
        //                @"
        //                    SELECT [UserName], [Email] 
        //                    FROM AspNetUsers 
        //                    WHERE [IsActive] = 1 AND [EmailConfirmed] = 0
        //                ",
        //                stoppingToken
        //            );

        //            if (users.Any())
        //            {
        //                _logger.LogInformation($"📢 Found {users.Count} users to notify!");

        //                foreach (var user in users)
        //                {
        //                    var emailPayload = new EmailRequest
        //                    {
        //                        To = user.Email,
        //                        Subject = "Email Confirmation Required",
        //                        Body = $"Hello {user.UserName}, please confirm your email."
        //                    };

        //                    string jsonPayload = JsonSerializer.Serialize(emailPayload);
        //                    _emailService.SendEmail(jsonPayload);

        //                    _logger.LogInformation($"📧 Email sent to {user.Email}");
        //                }
        //            }
        //            else
        //            {
        //                _logger.LogInformation("✅ No pending email confirmations.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"❌ Error while sending emails: {ex.Message}");
        //    }

            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        //}
    }
}
