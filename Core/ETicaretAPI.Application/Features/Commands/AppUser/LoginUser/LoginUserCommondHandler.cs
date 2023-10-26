using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommondHandler : IRequestHandler<LoginUserCommondRequest, LoginUserCommondResponse>
    {
        IAuthService _service;
        public LoginUserCommondHandler(IAuthService service)
        {
            _service = service;
        }
        public async Task<LoginUserCommondResponse> Handle(LoginUserCommondRequest request, CancellationToken cancellationToken)
        {
            Token token = await _service.LoginAsync(request.UsernameOrEmail, request.Password, 900);
            return new LoginUserSuccessCommondResponse()
            {
                Token = token,
            };
        }
    }
}
