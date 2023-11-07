using Azure.Core;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Domain.Entites.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entites.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<Domain.Entites.Identity.AppUser> _signInManager;
        readonly IUserService _userService;
        readonly IMailService _mailService;

        public AuthService(UserManager<Domain.Entites.Identity.AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<Domain.Entites.Identity.AppUser> signInManager, IUserService userService, IMailService mailService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
        }
        public async Task<Token> GoogleLoginAsync(string IdToken,int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { $"{_configuration["ExternalLoginSettings:Google:Client_ID"]}" },
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(IdToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            Domain.Entites.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        NameSurname = payload.Name
                    };
                    var identityRsult = await _userManager.CreateAsync(user);
                    result = identityRsult.Succeeded;
                }
            }
            if (result)
            {
                await _userManager.AddLoginAsync(user, info);
            }
            else
            {
                throw new Exception("problem var hocam");
            }

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken,user,token.Expiration,10);
            return token;
        }

        public async Task<Token> LoginAsync(string UsernameOrEmail, string password, int accessTokenLifeTime)
        {

            Domain.Entites.Identity.AppUser user = await _userManager.FindByNameAsync(UsernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(UsernameOrEmail);

            if (user == null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 10);
                return token;
            }
            throw new AuthenticationErrorException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
           AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow.AddHours(3))
            {
                Token token = _tokenHandler.CreateAccessToken(15,user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 300);
                return token;
            }
            else
                throw new NotFoundUserException();
        }

        public async Task PasswordResetAsync(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if(user!= null)
            {
                string resetToken =  await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken = CustomEncoders.UrlEncode(resetToken);
                await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
            }
            
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if(user!=null)
            {
               resetToken = CustomEncoders.UrlDecode(resetToken);

               return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider,"ResetPassword",resetToken);
            }
            return false;
        }
    }
}
