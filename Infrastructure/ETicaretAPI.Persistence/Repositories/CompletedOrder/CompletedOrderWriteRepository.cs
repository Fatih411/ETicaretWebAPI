using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.CompletedOrder;
using ETicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories.CompletedOrder
{
    public class CompletedOrderWriteRepository : WriteRepository<Domain.Entites.CompletedOrder>, ICompletedOrderWriteRepository
    {
        public CompletedOrderWriteRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
