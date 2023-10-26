using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.ProductImageFile;
using ETicaretAPI.Domain.Entites;
using ETicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories
{
    public class ProductImageWriteRepostiory : WriteRepository<ProductImagesFile>, IProductImageFileWriteRepository
    {
        public ProductImageWriteRepostiory(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
