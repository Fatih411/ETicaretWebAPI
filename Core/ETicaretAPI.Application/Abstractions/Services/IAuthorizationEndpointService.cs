using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IAuthorizationEndpointService
    {
        Task<List<string>> GetRolesToEndpoint(string menu,string code);
        Task AssignRoleEndpointAsync(string[] roles,string menu, string code, Type type);
    }
}
