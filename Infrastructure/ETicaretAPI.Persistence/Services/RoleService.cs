using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Domain.Entites.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public (object, int) GetAllRoles(int page,int size)
        {
            var query = _roleManager.Roles;

            IQueryable<AppRole> rolesQuery = null;

            if (page != -1 && size != 1)
                rolesQuery = query.Skip(page * size).Take(size);
            else
                rolesQuery = query;
      
            return (rolesQuery.Select(role => new { role.Id, role.Name }), query.Count());
        }

        public async Task<(string Id, string name)> GetByIdRole(string id)
        {
           string role = await _roleManager.GetRoleIdAsync(new() { Id=id});
           return (id, role);
        }

        public async Task<bool> CreateRole(string name)
        {
            IdentityResult result = await _roleManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
            });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string Id)
        {
            AppRole appRole = await _roleManager.FindByIdAsync(Id);
            IdentityResult result = await _roleManager.DeleteAsync(appRole);
            return result.Succeeded;
        }

        public async Task<bool> UpdateRole(string Id,string name)
        {
            AppRole appRole = await _roleManager.FindByIdAsync(Id);
            appRole.Name = name;
            IdentityResult result = await _roleManager.UpdateAsync(appRole);
            return result.Succeeded;
        }
    }
}
