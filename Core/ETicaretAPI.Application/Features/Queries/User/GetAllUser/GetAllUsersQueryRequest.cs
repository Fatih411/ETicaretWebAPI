using MediatR;

namespace ETicaretAPI.Application.Features.Queries.User.GetAllUser
{
    public class GetAllUsersQueryRequest: IRequest<GetAllUsersQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 0;
    }
}