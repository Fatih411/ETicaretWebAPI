using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.UpdatePassword
{
    public class UpdatePasswordCommandRequest : IRequest<UpdatePasswordCommandResponse>
    {
        public string UserId { get; set; }
        public string resetToken { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

    }
}