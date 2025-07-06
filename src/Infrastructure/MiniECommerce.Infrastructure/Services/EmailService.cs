using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Email;

namespace MiniECommerce.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpOptions)
    {
        _smtpSettings = smtpOptions.Value;
    }

    public async Task SendAsync(EmailDto dto)
    {
        using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.From),
            Subject = dto.Subject,
            Body = dto.Body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(dto.To);

        await client.SendMailAsync(mailMessage);
    }
}
