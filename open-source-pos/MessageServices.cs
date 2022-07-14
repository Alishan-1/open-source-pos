using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;
//using Twilio.Types;

namespace Services
{
    
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly EmailConfig ec;
        private readonly SMSoptions so;

        public AuthMessageSender(IOptions<EmailConfig> emailConfig, IOptions<SMSoptions> smsOptions)
        {
            this.ec = emailConfig.Value;
            this.so = smsOptions.Value;
        }
        public async Task SendEmailAsync(String email, String subject, String message)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(ec.FromName, ec.FromAddress));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                using (var client = new SmtpClient())
                {
                    client.LocalDomain = ec.LocalDomain;
                    // NEVER_EAT_POISON_Disable_CertificateValidation();
                    await client.ConnectAsync(ec.MailServerAddress, Convert.ToInt32(ec.MailServerPort), SecureSocketOptions.SslOnConnect).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error sending email: "+ex.Message);
                throw new Exception("error sending email: " + ex.Message);
            }
        }

        // [Obsolete("Do not use this in Production code!!!", true)]
        void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                ) {
                    return true;
                };
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            var accountSid = so.accountSid;
            // Your Auth Token from twilio.com/console
            var authToken = so.authToken;

            //TwilioClient.Init(accountSid, authToken);

            //var msg = MessageResource.Create(
            //  to: new PhoneNumber(number),
            //  from: new PhoneNumber("+16366141210"), //"+15005550006"), test number
            //  body: message);

            return Task.FromResult(0);
        }
    }

    public class EmailConfig
    {
        public String FromName { get; set; }
        public String FromAddress { get; set; }

        public String LocalDomain { get; set; }

        public String MailServerAddress { get; set; }
        public String MailServerPort { get; set; }

        public String UserId { get; set; }
        public String UserPassword { get; set; }
    }

    public class SMSoptions
    {
        public string accountSid { get; set; }
        public string authToken { get; set; }
    }
}
