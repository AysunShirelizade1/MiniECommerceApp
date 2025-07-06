using MiniECommerce.Application.DTOs.Email;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IEmailService
{
    Task SendAsync(EmailDto dto);
}
