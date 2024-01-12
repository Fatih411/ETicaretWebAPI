using Azure.Core;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Application.Repositories.Endpoint;
using ETicaretAPI.Domain.Entites;
using ETicaretAPI.Domain.Entites.Identity;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
        readonly IEndpointReadRepository _endpointReadRepository;


        public UserService(UserManager<Domain.Entites.Identity.AppUser> userManager, ILogger<UserService> logger, IEndpointReadRepository endpointReadRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _endpointReadRepository = endpointReadRepository;
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
        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user,DateTime accessTokenDate,int addOnAccessTokenDate)
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
        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
               
                resetToken = CustomEncoders.UrlDecode(resetToken);
                IdentityResult result = await _userManager.ResetPasswordAsync(user,resetToken,newPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                }
                else
                {
                    throw new Exception("Şifre güncellerken bir hata yaşandı");
                }
            }
        }

        public async Task<List<ListUser>> GetAllUsers(int page,int size)
        {
           List<AppUser> users =  await _userManager.Users.Skip(page*size).Take(size).ToListAsync();
            return users.Select(u => new ListUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
            }).ToList();
        }

        public async Task AssignRoleToUserAsync(string userId, string[] roles)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if(user != null )
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                foreach (var role in roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public async Task<string[]> GetRolesToUser(string userIdOrName)
        {
            AppUser user = await _userManager.FindByIdAsync(userIdOrName);

            if(user == null)
                user = await _userManager.FindByNameAsync(userIdOrName);
            
            if(user!=null )
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return new string[] { };
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string userName, string code)
        {
            var userRoles = await GetRolesToUser(userName);
            if (!userRoles.Any())
                return false;

            Endpoint? endpoint = await _endpointReadRepository.Table.Include(e => e.Roles).FirstOrDefaultAsync(e => e.Code == code);

            if(endpoint == null)
                return false;

            var hasRole = false;
            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            /* foreach (var userRole in endpointRoles)
             {
                 if (!hasRole)
                 {
                     foreach (var endpointRole in endpointRoles)
                     {
                         if (userRole == endpointRole)
                         {
                             hasRole = true;
                             break;
                         }
                     }
                 }
                 else
                     break;

             }
             return hasRole;*/
            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;
            }
            return false;
        }

        public int TotalUsersCount => _userManager.Users.Count();
    }
}
