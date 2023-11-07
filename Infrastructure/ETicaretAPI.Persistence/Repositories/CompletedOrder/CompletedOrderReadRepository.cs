using ETicaretAPI.Application.Repositories.CompletedOrder;
using ETicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories.CompletedOrder
{
    public class CompletedOrderReadRepository : ReadRepository<Domain.Entites.CompletedOrder>, ICompletedOrderReadRepository
    {
        public CompletedOrderReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
