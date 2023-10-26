using Azure.Core;
using ETicaretAPI.Application.Abstractions.Storage;
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

        [Authorize(AuthenticationSchemes = "admin")]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute]GetByIdProductRequest getByIdProductRequest)
        {
            GetByIdProductResponse response = await _mediator.Send(getByIdProductRequest);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
          
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [HttpGet("[action]/{productId}")]
        public async Task<IActionResult> GetProductImages([FromRoute]GetProductImageQureyRequest getProductImageQureyRequest)
        {
            List<GetProductImageQueryResponse> response = await _mediator.Send(getProductImageQureyRequest);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "admin")]
        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery]string imageId)
        {
            removeProductImageCommandRequest.imageId= imageId;
            await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery]ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
           ChangeShowcaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }
        
    }
}