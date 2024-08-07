using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using UdemyEgitimPlatformu.Data;
using UdemyEgitimPlatformu.Models;

namespace UdemyEgitimPlatformu.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly ApplicationDbContext _context;
        private readonly SmtpSettings _smtpSettings;

        private readonly string _smtpserver;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public EmailSender(IConfiguration configuration, ApplicationDbContext context)
        {
            _smtpSettings = context.SmtpSettings.FirstOrDefault();

            _context = context;

            if (_smtpSettings == null)
            {
                throw new InvalidOperationException("SMTP settings are not configured.");
            }

            //_smtpserver = configuration["SmtpSettings:Server"];
            //_port = int.Parse(configuration["SmtpSettings:Port"]);
            //_username = configuration["SmtpSettings:Username"];
            //_password = configuration["SmtpSettings:Password"];
            _context = context;

            if (_smtpSettings == null)
            {
                throw new InvalidOperationException("SMTP settings are not configured.");
            }

        }
           


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = _smtpSettings.EnableSsl;

                await smtpClient.SendMailAsync(mailMessage);
            }

            //var MailMesaj = new MailMessage
            //{
            //    From = new MailAddress(_username),
            //    Subject = subject,
            //    Body = htmlMessage,
            //    IsBodyHtml = true,

            //};

            //MailMesaj.To.Add(email);

            //using(var smtpClient= new SmtpClient(_smtpserver, _port))
            //{
            //    smtpClient.Credentials = new NetworkCredential(_username, _password);
            //    smtpClient.EnableSsl = true;


            //    await smtpClient.SendMailAsync(MailMesaj);
            //}

        }
    }
}
