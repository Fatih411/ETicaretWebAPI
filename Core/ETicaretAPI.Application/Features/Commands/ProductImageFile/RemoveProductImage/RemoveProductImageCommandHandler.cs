using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entites;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest,RemoveProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;
        public RemoveProductImageCommandHandler(IStorageServices storageServices, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entites.Product? product = await _productReadRepository.Table.Include(p => p.ProductImagesFiles)
               .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
            Domain.Entites.ProductImagesFile? productImageFile = product?.ProductImagesFiles?.FirstOrDefault(p => p.Id == Guid.Parse(request.imageId));
            product?.ProductImagesFiles?.Remove(productImageFile!);
            await _productWriteRepository.SaveAsync();
            return new();
        }
    }
}
