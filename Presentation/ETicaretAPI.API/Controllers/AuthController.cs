using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Application.Features.Commands.AppUser.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommondRequest loginUserCommondRequest)
        {
            LoginUserCommondResponse response = await _mediator.Send(loginUserCommondRequest);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody]RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(response);
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommondRequest googleLoginCommondRequest)
        {
            GoogleLoginCommondResponse response = await _mediator.Send(googleLoginCommondRequest);
            return Ok(response);
        }
    }
}
