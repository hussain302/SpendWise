namespace Application.Services;

public interface IEmailService
{
    void SendEmail(string jsonPayload);
}