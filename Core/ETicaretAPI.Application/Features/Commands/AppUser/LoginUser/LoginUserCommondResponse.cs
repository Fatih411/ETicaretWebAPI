using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommondResponse
    {
        
        
    }
    public class LoginUserSuccessCommondResponse : LoginUserCommondResponse
    {
        public Token Token { get; set; }
    }
    public class LoginUserErrorCommondResponse : LoginUserCommondResponse 
    {
        public string Message { get; set; }
    }

}
