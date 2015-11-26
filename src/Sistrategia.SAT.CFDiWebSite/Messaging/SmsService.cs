using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Configuration;

namespace Sistrategia.SAT.CFDiWebSite.Messaging
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message) {
            //string AccountSid = "YourTwilioAccountSID";
            //string AuthToken = "YourTwilioAuthToken";
            //string twilioPhoneNumber = "YourTwilioPhoneNumber";

            //var twilio = new TwilioRestClient(AccountSid, AuthToken);

            //twilio.SendSmsMessage(twilioPhoneNumber, message.Destination, message.Body);

            // Twilio does not return an async Task, so we need this:
            return Task.FromResult(0);
        }
    }
}
