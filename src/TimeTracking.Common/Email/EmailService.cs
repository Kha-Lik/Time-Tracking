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

        public async Task<bool> SendMessage (MailModel model)
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
             
                _logger.LogError($"{DateTime.Now}: Failed to send email with purpose {emailPurpose.ToString()} ❌! ({ex.Message})");
                return false;
            }
        }
    

    /* public async Task SendAsync(string EmailDisplayName, string Subject, string Body, string From, string To)
     {
         await SendSendGridMessage(From, EmailDisplayName, new List<EmailAddress> { new EmailAddress(To) }, Subject, Body).ConfigureAwait(false);
     }

     public async Task SendEmailConfirmationAsync(string EmailAddress, string CallbackUrl)
     {
         var Subject = "Confirm your email";
         var HTMLContent = $"Please confirm your email by clicking here: <a href='{CallbackUrl}'>link</a>";

         await SendSendGridMessage(_email.From, _email.DisplayName, new List<EmailAddress> { new EmailAddress(EmailAddress) }, Subject, HTMLContent).ConfigureAwait(false);
     }

     public async Task SendPasswordResetAsync(string EmailAddress, string CallbackUrl)
     {
         var Subject = "Reset your password";
         var HTMLContent = $"Please reset your password by clicking here: <a href='{CallbackUrl}'>link</a>";

         await SendSendGridMessage(_email.From, _email.DisplayName, new List<EmailAddress> { new EmailAddress(EmailAddress) }, Subject, HTMLContent).ConfigureAwait(false);
     }

     public async Task SendException(Exception ex)
     {
         var Subject = $"[{_env.EnvironmentName}] INTERNAL SERVER ERROR";
         var HTMLContent = $"{ex.ToString()}";

         await SendSendGridMessage(_email.From, _email.DisplayName, new List<EmailAddress> { new EmailAddress(_email.To) }, Subject, HTMLContent).ConfigureAwait(false);
     }

     public async Task<> SendSqlException(SqlException ex)
     {
         var Subject = $"[{_env.EnvironmentName}] SQL ERROR";
         var HTMLContent = $"{ex.ToString()}";

         await SendSendGridMessage(_email.From, _email.DisplayName, new List<EmailAddress> { new EmailAddress(_email.To) }, Subject, HTMLContent).ConfigureAwait(false);
     }

     private async Task SendSendGridMessage(string From, string EmailDisplayName, List<EmailAddress> tos, string Subject, string HTMLContent)
     {
         var client = new SendGridClient(_email.SendGridApiKey);
         var from = new EmailAddress(From, EmailDisplayName);
         var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, Subject, "", HTMLContent, false);
         var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
     }*/
    }
}