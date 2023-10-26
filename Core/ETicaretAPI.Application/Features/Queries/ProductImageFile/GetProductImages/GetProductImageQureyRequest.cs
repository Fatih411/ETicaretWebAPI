using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImageQureyRequest : IRequest<List<GetProductImageQueryResponse>>
    {
        public string productId { get; set; }
    }
}
