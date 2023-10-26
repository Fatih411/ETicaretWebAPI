using ETicaretAPI.Domain.Entites.Common;
using ETicaretAPI.Domain.Entites.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entites
{
    public class Basket : BaseEntity
    {
        public string UserId { get; set; }
        public Order Order { get; set; }
        public AppUser User { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
