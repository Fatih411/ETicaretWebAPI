using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Order
{
    public class sendMailOrder
    {
        public string NameSurname { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OrderCode { get; set; }
        public string Email { get; set; }
    }
}
