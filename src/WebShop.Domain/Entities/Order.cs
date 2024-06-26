using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public required List<OrderItem> OrderItems { get; set; }
        public required string OrderReferenceId { get; set; }
    }
}
