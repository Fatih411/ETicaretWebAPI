using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductHandler : IRequestHandler<GetByIdProductRequest, GetByIdProductResponse>
    {
        readonly IProductReadRepository _productReadRepository;

        public GetByIdProductHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetByIdProductResponse> Handle(GetByIdProductRequest request, CancellationToken cancellationToken)
        {
            ETicaretAPI.Domain.Entites.Product products = await _productReadRepository.GetByIdAsync(request.Id,false);
            return new()
            {
                Id=products.Id.ToString(),
                Name=products.Name,
                Price=products.Price,
                Stock=products.Stock,
            };
        }
    }
}
