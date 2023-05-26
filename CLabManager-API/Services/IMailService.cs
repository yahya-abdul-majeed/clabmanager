using ModelsLibrary.Models;

namespace CLabManager_API.Services
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
        Task<bool> SendWithAttachmentAsync(MailDataWithAttachments mailData, CancellationToken ct);
        string GetEmailTemplate<T>(string emailTemplate, T emailTemplateModel);
    }
}
