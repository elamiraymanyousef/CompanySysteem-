using Company.PL.HelperImage;
using Company.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace Company.PL.Helper
{
    public class MailService : IMailService
    {
        private readonly MailSettings _options;

        public MailService(IOptions<MailSettings> options) 
         {
            _options = options.Value;
        }
        public void SendEmail(Email email)
        {
            //Build Message
            var mail = new MimeMessage();
            mail.Subject = email.Subject;
            //mail.From.Add(MailboxAddress.Parse( _options.Email));// dispaly Email only in message
            mail.From.Add(new MailboxAddress(_options.DisplayName,_options.Email ));// display name and email in message

            mail.To.Add(MailboxAddress.Parse(_options.Email));

            var builder  = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            //Establish Connection
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);

            //Send message
           smtp.Send(mail);
        }
    }
}
