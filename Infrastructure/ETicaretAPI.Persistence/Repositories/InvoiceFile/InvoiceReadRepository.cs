using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.InvoiceFile;
using ETicaretAPI.Domain.Entites;
using ETicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories
{
    public class InvoiceReadRepository : ReadRepository<InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
