using Microsoft.AspNet.Identity;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Escrow.BOAS.Utility
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            try
            {
                await configSendGridasync(message);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            try
            {
                var myMessage = new SendGridMessage();
                myMessage.AddTo(message.Destination);
                myMessage.From = new System.Net.Mail.MailAddress(
                                    ConfigManager.Email_FromAddress, 
                                    ConfigManager.Email_FromDisplayName);
                myMessage.Subject = message.Subject;
                myMessage.Text = message.Body;
                myMessage.Html = message.Body;

                var credentials = new NetworkCredential(
                           ConfigManager.Email_User,
                           ConfigManager.Email_Password
                           );

                // Create a Web transport for sending email.
                var transportWeb = new SendGrid.Web(credentials);

                // Send the email.
                if (transportWeb != null)
                {
                    await transportWeb.DeliverAsync(myMessage);
                }
                else
                {
                    await Task.FromResult(0);
                    throw new Exception("Failed to create Web transport.");
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
    }
}