using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Domain.Entites.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        Application.DTOs.Token CreateAccessToken(int second,AppUser user);
        string CreateRefreshToken();
    }
}
