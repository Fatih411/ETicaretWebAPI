using ETicaretAPI.Application.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Role.GetByIdRoles
{
    public class GetByIdRolesQueryHandler : IRequestHandler<GetByIdRolesQueryRequest, GetByIdRolesQueryResponse>
    {
        readonly IRoleService _roleService;

        public GetByIdRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetByIdRolesQueryResponse> Handle(GetByIdRolesQueryRequest request, CancellationToken cancellationToken)
        {
           var data = await _roleService.GetByIdRole(request.Id);
            return new()
            {
                Id = data.Id,
                Name = data.name
            };
        }
    }
}
