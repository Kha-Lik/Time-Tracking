using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Services;
using TimeTracking.Templates.Services;

namespace TimeTracking.Common.Email
{
    public static class AddFluentEmailDependency
    {
            public static IServiceCollection AddFluentEmailServices(this IServiceCollection services,IConfiguration configuration)
            {
                services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
                var emailSettings = new EmailSettings();
                configuration.GetSection(nameof(EmailSettings)).Bind(emailSettings);
                
                services.AddFluentEmail(emailSettings.AdminEmail)
                    .AddRazorRenderer()
                    .AddSmtpSender(new SmtpClient(emailSettings.MailServer,emailSettings.MailPort)
                    {
                        Credentials = new NetworkCredential(emailSettings.AdminEmail, emailSettings.AdminPassword),
                        EnableSsl = emailSettings.UseSsl,
                    });
                
                services.AddScoped<IEmailService, EmailService>();
                services.AddScoped<IRazorViewFinder, RazorViewFinder>();
                services.RegisterTemplateServices();
                services.AddRazorPages();
                return services;
            }
    }
}