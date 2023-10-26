using ETicaretAPI.Application.Repositories.Basket;
using ETicaretAPI.Application.Repositories.BasketItem;
using ETicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories.Basket
{
    public class BasketReadRepository : ReadRepository<Domain.Entites.Basket>, IBasketReadRepository
    {
        public BasketReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
