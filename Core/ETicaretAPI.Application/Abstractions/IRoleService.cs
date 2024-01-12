using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions
{
    public interface IRoleService
    {
        (object,int) GetAllRoles(int page,int size);
        Task<(string Id, string name)> GetByIdRole(string id);
        Task<bool> CreateRole(string name);
        Task<bool> DeleteRole(string Id);
        Task<bool> UpdateRole(string Id,string name);

    }
}
