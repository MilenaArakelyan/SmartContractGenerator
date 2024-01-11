namespace ProjectS5.Core.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(string receiverEmail, string subject, string body, bool isHtmlContent = false, CancellationToken cancellationToken = default);
}