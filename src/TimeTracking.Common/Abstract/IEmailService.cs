using System.Threading.Tasks;
using TimeTracking.Common.Email;
using TimeTracking.Common.Enums;

namespace TimeTracking.Common.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendMessageWithPurpose<TModel>(MailModel model, EmailPurpose emailPurpose, TModel templateModel)
            where TModel : class;

        Task<bool> SendMessage(MailModel model);
    }
}