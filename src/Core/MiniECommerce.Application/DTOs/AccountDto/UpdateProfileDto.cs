using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniECommerce.Application.DTOs.AccountDto;

public class UpdateProfileDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
