﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.ViewModels.Basket
{
    public class VM_Update_BasketItem
    {
        public Guid BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
