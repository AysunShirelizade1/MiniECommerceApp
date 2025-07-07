using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniECommerce.Application.DTOs.RefreshTokenDto;

public class RefreshTokenRequest
{
    public string Token { get; set; } = null!;
}
