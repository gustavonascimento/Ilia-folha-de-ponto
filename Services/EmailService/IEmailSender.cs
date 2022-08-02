using System;
using System.Threading.Tasks;

namespace WebApi.Services.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
