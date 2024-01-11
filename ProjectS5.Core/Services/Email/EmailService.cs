using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ProjectS5.Core.Services.Email;

internal class EmailService(IOptionsMonitor<EmailSettings> emailOptionsMonitor, ISendGridClient sendGridClient) : IEmailService
{
    public async Task SendEmailAsync(string receiverEmail, string subject, string body, bool isHtmlContent = false, CancellationToken cancellationToken = default)
    {
        var email = new SendGridMessage
        {
            From = new EmailAddress(emailOptionsMonitor.CurrentValue.SenderEmail, string.Empty),
            Subject = subject,
            PlainTextContent = isHtmlContent ? string.Empty : body,
            HtmlContent = isHtmlContent ? body : string.Empty
        };
        email.AddTo(new EmailAddress(receiverEmail));
        var response = await sendGridClient.SendEmailAsync(email, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Body.ReadAsStringAsync(cancellationToken));
        }
    }
}