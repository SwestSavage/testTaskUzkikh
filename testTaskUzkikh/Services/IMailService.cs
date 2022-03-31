using testTaskUzkikh.Models;

namespace testTaskUzkikh.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(Email mailRequest);
    }
}
