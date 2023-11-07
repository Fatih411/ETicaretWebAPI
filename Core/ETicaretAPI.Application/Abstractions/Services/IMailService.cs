using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string body, string subject,string to, bool isBodyHtml = true);
        Task SendMailAsync(string body, string subject,string[] tos, bool isBodyHtml = true);
        Task SendPasswordResetMailAsync(string to, string userId, string resetToken);
        Task SendCompletedOrderMailAsync(string to,string orderCode, DateTime orderDate,string nameSurname);
    }
}
