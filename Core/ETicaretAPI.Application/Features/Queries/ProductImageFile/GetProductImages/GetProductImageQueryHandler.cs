using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImageQueryHandler : IRequestHandler<GetProductImageQureyRequest, List<GetProductImageQueryResponse>>
    {
       
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration _configuration;

        public GetProductImageQueryHandler(IConfiguration configuration, IProductReadRepository productReadRepository)
        {
      
            _configuration = configuration;
            _productReadRepository = productReadRepository;
        }

        public async Task<List<GetProductImageQueryResponse>> Handle(GetProductImageQureyRequest request, CancellationToken cancellationToken)
        {

            ETicaretAPI.Domain.Entites.Product? product = await _productReadRepository.Table.Include(p => p.ProductImagesFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.productId));
            return product?.ProductImagesFiles.Select(p => new GetProductImageQueryResponse
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                FileName = p.FileName,
                Id = p.Id,
                Showcase=p.Showcase
            }).ToList();
        }
    }
}
