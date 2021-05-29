using System;
using System.Threading.Tasks;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Enums;

namespace TimeTracking.Common.Email
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _email;
        private readonly ILogger<EmailService> _logger;
        private readonly IReadOnlyTemplateStorageService _templateStorageService;
        private const string TemplatePath = "Web.Api.Infrastructure.Services.Emails.Templates.{0}.cshtml";

        public EmailService(IFluentEmail email,
            ILogger<EmailService> logger,
            IReadOnlyTemplateStorageService templateStorageService)
        {
            _email = email;
            _logger = logger;
            _templateStorageService = templateStorageService;
        }

        public async Task<bool> SendMessage(MailModel model)
        {
            try
            {
                var result = await _email
                    .To(model.ToEmail)
                    .Subject(model.Subject)
                    .Body(model.Body)
                    .SendAsync();
                if (!result.Successful)
                {
                    _logger.LogError("Failed to send an email.\n{Errors}",
                        string.Join(Environment.NewLine, result.ErrorMessages));
                }

                return result.Successful;
            }
            catch (Exception ex)
            {

                _logger.LogError($"{DateTime.Now}: Failed to send email notification ❌! ({ex.Message})");
                return false;
            }

        }

        public async Task<bool> SendMessageWithPurpose<TModel>(MailModel model, EmailPurpose emailPurpose,
            TModel templateModel)
            where TModel : class
        {
            try
            {
                var result = await _email
                    .To(model.ToEmail)
                    .Subject(model.Subject)
                    .UsingTemplateFromFile(_templateStorageService.GetPathByKey(emailPurpose.ToString()), templateModel)
                    .SendAsync();

                if (!result.Successful)
                {
                    _logger.LogError("Failed to send an email.\n{Errors}",
                        string.Join(Environment.NewLine, result.ErrorMessages));
                }

                return result.Successful;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,$"{DateTime.Now}: Failed to send email with purpose {emailPurpose.ToString()} ❌! ({ex.Message})");
                return false;
            }
        }

    }
}