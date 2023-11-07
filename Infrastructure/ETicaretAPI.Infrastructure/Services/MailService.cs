using ETicaretAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string body, string subject, string to, bool isBodyHtml = true)
        {
            await SendMailAsync(body,subject , new[] { to }, isBodyHtml);
        }

        public async Task SendMailAsync(string body, string subject, string[] tos, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.Body = body;
            mail.Subject = subject;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.IsBodyHtml = isBodyHtml;
            mail.From = new(_configuration["Mail:Username"],"Fatih E-Ticaret",System.Text.Encoding.UTF8);

            SmtpClient smpt = new();
            smpt.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smpt.Port = 587;
            smpt.EnableSsl=true;
            smpt.Host=_configuration["Mail:Host"];
            await smpt.SendMailAsync(mail);
            
            
        }
 
        public async Task SendPasswordResetMailAsync(string to,string userId,string resetToken)
        {
            StringBuilder mail = new StringBuilder();

            mail.AppendLine("Merhaba,");
            mail.AppendLine("Eğer yeni şifre talebinde bulunduysanız, aşağıdaki linkten şifrenizi yenileyebilirsiniz.");
            mail.AppendLine("Şifre Yenileme Linki:");
            mail.AppendLine(_configuration["AngularClientUrl"] + "/update-password/" + userId + "/" + resetToken);
            mail.AppendLine("Bu linki kullanarak şifrenizi güncelleyebilirsiniz.");
            mail.AppendLine("Eğer bu talep tarafınızca gerçekleştirilmediyse, bu maili ciddiye almayınız.");
            mail.AppendLine("İyi Günler.");
            mail.AppendLine("Fatih E-Ticaret");

            await SendMailAsync(mail.ToString(), "Şifre Yenileme Talebi", to);
        }
        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string nameSurname)
        {
            string mail = $"Değerli Müşterimiz {nameSurname}.<br>" +
                $"{orderDate} tarihinde yaptığınız {orderCode} numaralı alışverişiniz başarılı bir şekilde onaylanmıştır<br>"
                +"Bizi tercih ettiğiniz için teşekkür ederiz, iyi günler dileriz.<br>"+
                "Fatih ETicaret";

            await SendMailAsync(mail, $"{orderCode} Sipariş Onaylandı.", to);
        }
    }
}
