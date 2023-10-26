using Azure.Core;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using ETicaretAPI.Domain.Entites.Identity;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entites.Identity.AppUser> _userManager;
        readonly ILogger<UserService> _logger;
        public UserService(UserManager<Domain.Entites.Identity.AppUser> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser createUser)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = createUser.UserName,
                Email = createUser.Email,
                NameSurname = createUser.NameSurname,
            }, createUser.Password);
            CreateUserResponse response = new() { Succedded = result.Succeeded };
            if (result.Succeeded)
                response.Message = "Kayıt olma başarılı.";

            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} : {error.Description}\n";
                }
            }

            return response;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user,DateTime accessTokenDate,int addOnAccessTokenDate)
        {
            if (user != null) 
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                _logger.LogInformation("Refresh token güncellenmedi...");
                throw new NotFoundUserException();
                
            }
               
        }
   
        
    }
}
