using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Mail;


namespace CLabManager_API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        static bool mailSent = false;

        private static void SendCompletedCallback (object sender, AsyncCompletedEventArgs e)
        {
                string token = (string)e.UserState;
                if(e.Cancelled)
                {
                    Console.WriteLine("[{0}] Send Cancelled", token);
                }
                if(e.Error != null)
                {
                    Console.WriteLine("[{0}] {1}", token, e.Error.Message);
                }
                else
                {
                    Console.WriteLine("Message Sent.");
                }
                mailSent = true;

        }
        public void SendEmailTo(string email)
        {
            SmtpClient client = new SmtpClient();
            client.Host = _configuration["MailSettings:Host"];
            client.Port = int.Parse(_configuration["MailSettings:Port"]);
            client.EnableSsl = true;
            NetworkCredential NC = new NetworkCredential();
            NC.UserName = _configuration["MailSettings:UserName"]; 
            NC.Password = _configuration["MailSettings:Password"]; 
            client.Credentials = NC;
            MailAddress from = new MailAddress("yahya.reads.books@gmail.com", "yahya abdul majeed");
            MailAddress to = new MailAddress(email);

            MailMessage message = new MailMessage(from,to);
            message.Body = "A new issue has been created";
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "New Issue";

            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            client.Send(message);
            message.Dispose();
        }
    }
}
