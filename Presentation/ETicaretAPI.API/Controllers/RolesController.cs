using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Features.Commands.Role.CreateRole;
using ETicaretAPI.Application.Features.Commands.Role.DeleteRole;
using ETicaretAPI.Application.Features.Commands.Role.UpdateRole;
using ETicaretAPI.Application.Features.Queries.Role.GetByIdRoles;
using ETicaretAPI.Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="admin")]
    public class RolesController : ControllerBase
    {
        readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Reading,Definition ="Get Roles",Menu ="Roles")] 
        
        public async Task<IActionResult> GetRoles([FromQuery]GetRolesQueryRequest getRolesQueryRequest)
        {
            GetRolesQueryResponse response = await _mediator.Send(getRolesQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Reading, Definition = "Get Role By Id", Menu = "Roles")]
        public async Task<IActionResult> GetRoles([FromRoute]GetByIdRolesQueryRequest getByIdRolesQueryRequest)
        {
            GetByIdRolesQueryResponse response = await _mediator.Send(getByIdRolesQueryRequest);
            return Ok(response);
        }
        [HttpPost]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Writing, Definition = "Create Role", Menu = "Roles")]
        public async Task<IActionResult> CreateRoles([FromBody]CreateRoleCommandRequest createRoleCommandRequest)
        {
            CreateRoleCommandResponse response = await _mediator.Send(createRoleCommandRequest);
            return Ok(response);
        }
        [HttpPut("{Id}")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Updateing, Definition = "Update Role", Menu = "Roles")]
        public async Task<IActionResult> UpdateRoles([FromBody,FromRoute]UpdateRoleCommandRequest updateRoleCommandRequest)
        {
            UpdateRoleCommandResponse response = await _mediator.Send(updateRoleCommandRequest);
            return Ok(response);
        }
        [HttpDelete("{Id}")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Deleteing, Definition = "Delete Role", Menu = "Roles")]
        public async Task<IActionResult> DeleteRoles([FromRoute]DeleteRoleCommandRequest deleteRoleCommandRequest)
        {
            DeleteRoleCommandResponse response = await _mediator.Send(deleteRoleCommandRequest);
            return Ok(response);
        }
    }
}
