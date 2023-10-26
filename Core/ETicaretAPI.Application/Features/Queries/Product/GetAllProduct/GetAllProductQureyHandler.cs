using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using Google.Apis.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQureyHandler : IRequestHandler<GetAllProductQueryRequest, GetAllQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        public GetAllProductQureyHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }
        public async Task<GetAllQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                .Include(p=>p.ProductImagesFiles).Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdateDate,
                p.ProductImagesFiles
            }).ToList();

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}
