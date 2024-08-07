using System.Net.Mail;
using System.Net;
using UdemyEgitimPlatformu.Models;
using UdemyEgitimPlatformu.Services;
using System.Net.Sockets;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<EmailService> _logger;


    public EmailService(SmtpSettings smtpSettings, ILogger<EmailService> logger)
    {
        _smtpSettings = smtpSettings;
        _logger = logger;

    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient("smtp.office365.com")
        {
            Host = _smtpSettings.Host,
            Port = _smtpSettings.Port,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.Username),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            _logger.LogInformation($"E-Mail başarıyla gönderildi:  {toEmail}.");
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, $"SMTP error occurred while sending email to {toEmail}.");
            throw;
        }
        catch (SocketException socketEx)
        {
            _logger.LogError(socketEx, $"Socket error occurred while sending email to {toEmail}.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"General error occurred while sending email to {toEmail}.");
            throw;
        }
    }
}
