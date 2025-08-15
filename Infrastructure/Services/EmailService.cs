using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Application.Services;
using Application.DTOs.Email;
using Microsoft.Extensions.Options;
using Infrastructure.Configurations;

namespace Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> emailSettings) 
    : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public void SendEmail(string jsonPayload)
    {
        try
        {
            var emailData = JsonSerializer.Deserialize<EmailRequest>(jsonPayload)
                ?? throw new ApplicationException("Email data is null");

            if (string.IsNullOrWhiteSpace(emailData.To))
                throw new ArgumentException("Recipient email (To) cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(_emailSettings.From))
                throw new ApplicationException("Sender email (From) is not configured properly.");

            using MailMessage mail = new()
            {
                From = new MailAddress(_emailSettings.From),
                Subject = emailData.Subject,
                Body = emailData.Body,
                IsBodyHtml = true
            };

            mail.To.Add(emailData.To);

            using SmtpClient smtp = new(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password),
                EnableSsl = true
            };

            smtp.Send(mail);
            Console.WriteLine("✅ Email Sent Successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Email Sending Failed: {ex.Message}");
        }
    }

}
