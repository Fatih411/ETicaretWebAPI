using Azure.Core;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.Product.CreateProduct;
using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.CangeShowCaseImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Application.Repositories.InvoiceFile;
using ETicaretAPI.Application.Repositories.ProductImageFile;
using ETicaretAPI.Application.RequestParameters;

using ETicaretAPI.Application.ViewModels.Product;
using ETicaretAPI.Domain.Entites;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using File = ETicaretAPI.Domain.Entites.File;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductsController : ControllerBase
    {
        readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {

            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
           GetAllQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }
        
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute]GetByIdProductRequest getByIdProductRequest)
        {
            GetByIdProductResponse response = await _mediator.Send(getByIdProductRequest);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Product, ActionType = ActionType.Writing, Definition = "Create Product")]
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Product, ActionType = ActionType.Updateing, Definition = "Update Product")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Product, ActionType = ActionType.Deleteing, Definition = "Delete Product")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Product, ActionType = ActionType.Writing, Definition = "Upload Product File")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
          
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Product, ActionType = ActionType.Reading, Definition = "Get Product Images")]
        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetProductImages([FromRoute]GetProductImageQureyRequest getProductImageQureyRequest)
        {
            List<GetProductImageQueryResponse> response = await _mediator.Send(getProductImageQureyRequest);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Product, ActionType = ActionType.Deleteing, Definition = "Delete Product File")]
        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery]string imageId)
        {
            removeProductImageCommandRequest.imageId= imageId;
            await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefiniitonConstants.Product, ActionType = ActionType.Updateing, Definition = "Product File Showcase")]
        [HttpGet("[action]")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery]ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
           ChangeShowcaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }
        
    }
}