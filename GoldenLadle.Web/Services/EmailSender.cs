using System;
using System.Threading.Tasks;
using GoldenLadle.Web.Extensions;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GoldenLadle.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public AuthMessageSenderOptions Options { get; }
        public Task SendEmailAsync(string email, string subject, string message) =>
            Execute(Options.SendGridKey, subject, message, email);

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ben@brinehartdesign.com", "Ben Rinehart");
            var to = new EmailAddress(email, email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message.ToString(), message);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"SendGrid Response: {response}");
        }
    }
}