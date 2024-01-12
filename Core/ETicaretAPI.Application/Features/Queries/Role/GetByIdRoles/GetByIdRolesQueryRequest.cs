using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Role.GetByIdRoles
{
    public class GetByIdRolesQueryRequest : IRequest<GetByIdRolesQueryResponse>
    {
        public string Id { get; set; }
    }
}