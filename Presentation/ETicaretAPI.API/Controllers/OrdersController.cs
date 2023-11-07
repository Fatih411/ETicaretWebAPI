using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.Order.CompleteOrder;
using ETicaretAPI.Application.Features.Commands.Order.CreateOrder;
using ETicaretAPI.Application.Features.Queries.Order.GetAllOrder;
using ETicaretAPI.Application.Features.Queries.Order.GetByIdOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "admin")]
    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Order, ActionType = ActionType.Writing, Definition = "Add Order")]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandReguest createOrderCommandReguest)
        {
            CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandReguest);
            return Ok(response);
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Order, ActionType = ActionType.Reading, Definition = "Get Order")]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest)
        {
            GetAllOrdersQueryResponse response = await _mediator.Send(getAllOrdersQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Order, ActionType = ActionType.Reading, Definition = "Get Order By Id ")]
        public async Task<IActionResult> GetAllOrders([FromRoute] GetByIdOrderQueryRequest getByIdOrderQueryRequest)
        {
            GetByIdOrderQueryResponse response = await _mediator.Send(getByIdOrderQueryRequest);
            return Ok(response);
        }

        [HttpGet("complete-order/{Id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Order, ActionType = ActionType.Updateing, Definition = "Completed Order")]
        public async Task<IActionResult> CompleteOrder([FromRoute]CompleteOrderCommandRequest completeOrderCommandRequest)
        {
            CompleteOrderCommandResponse response =  await _mediator.Send(completeOrderCommandRequest);
            return Ok(response);
        }
    }
}
