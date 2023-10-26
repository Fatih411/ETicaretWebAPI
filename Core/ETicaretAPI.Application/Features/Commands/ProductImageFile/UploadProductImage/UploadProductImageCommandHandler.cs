using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories.ProductImageFile;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IStorageServices _storageServices;
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UploadProductImageCommandHandler(IStorageServices storageServices, IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _storageServices = storageServices;
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageServices.UplaodAsync("products", request.Files);
            Domain.Entites.Product product = await _productReadRepository.GetByIdAsync(request.ProductsId);
            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new Domain.Entites.ProductImagesFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageServices.StorageName,
                Products = new List<Domain.Entites.Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
