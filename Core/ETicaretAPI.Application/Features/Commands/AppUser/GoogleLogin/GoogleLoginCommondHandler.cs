using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommondHandler : IRequestHandler<GoogleLoginCommondRequest, GoogleLoginCommondResponse>
    {

        readonly IAuthService _authService;
        public GoogleLoginCommondHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<GoogleLoginCommondResponse> Handle(GoogleLoginCommondRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.GoogleLoginAsync(request.IdToken, 900);
            return new()
            {
                Token = token,
            };
        }
    }
}
