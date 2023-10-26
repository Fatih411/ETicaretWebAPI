﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductRequest : IRequest<GetByIdProductResponse>
    {
        public string Id { get; set; }
    }
}
