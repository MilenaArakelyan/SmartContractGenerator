using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectS5.Core.Auth;
using ProjectS5.Core.Services.Email;
using SendGrid.Extensions.DependencyInjection;

namespace ProjectS5.Core.ServiceExtensions;

public static class ServiceExtensions
{
    public static void RegisterCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthManager, AuthManager>();
    }

    public static void AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSendGrid(options =>
        options.ApiKey = configuration.GetSection(nameof(EmailSettings))
            .GetValue<string>(nameof(EmailSettings.ApiKey)));

        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
        services.AddScoped<IEmailService, EmailService>();
    }
}
