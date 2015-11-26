using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;
using System.Configuration;

namespace Sistrategia.SAT.CFDiWebSite.Messaging
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message) {
            var useSendGrid = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSendGrid"]);
            if (useSendGrid)
                return ConfigSendGridAsync(message);
            else
                return ConfigSendSMTPAsync(message);
        }

        private Task ConfigSendSMTPAsync(IdentityMessage message) {
            // string text = string.Format("Please click on this link to {0}: {1}", message.Subject, message.Body);
            // string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br/>";

            var myMessage = new System.Net.Mail.MailMessage();
            myMessage.To.Add(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AccountMailFrom"], ConfigurationManager.AppSettings["AccountMailFromDisplayName"]);
            myMessage.Subject = message.Subject;
            myMessage.AlternateViews.Add(System.Net.Mail.AlternateView.CreateAlternateViewFromString(message.Body, null, System.Net.Mime.MediaTypeNames.Text.Plain));
            myMessage.AlternateViews.Add(System.Net.Mail.AlternateView.CreateAlternateViewFromString(message.Body, null, System.Net.Mime.MediaTypeNames.Text.Html));

            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(
                ConfigurationManager.AppSettings["SMTPHost"],
                Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"])
                );
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["SMTPUserName"]
                , ConfigurationManager.AppSettings["SMTPPassword"]);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            // smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            // return smtpClient.SendMailAsync(myMessage);
            smtpClient.Send(myMessage);
            return Task.FromResult(0);
        }

        private Task ConfigSendGridAsync(IdentityMessage message) {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AccountMailFrom"], ConfigurationManager.AppSettings["AccountMailFromDisplayName"]);
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            //var credentials = new System.Net.NetworkCredential(
            //    ConfigurationManager.AppSettings["SendGridUser"],
            //    ConfigurationManager.AppSettings["SendGridAPIKey"]
            //    );

            //var transportWeb = new Web(credentials); // Cambiar por APIKEY
            var transportWeb = new Web(ConfigurationManager.AppSettings["SendGridAPIKey"]);

            if (transportWeb != null) {
                return transportWeb.DeliverAsync(myMessage);
            }
            else {
                return Task.FromResult(0);
            }
        }
    }
}