using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Entities;

namespace WebShop.Domain.Events
{
    public class OrderPlacedEvent
    {
        public Guid OrderId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public required List<OrderItem> OrderItems { get; set; }
        public required string OrderReferenceId { get; set; }
    }
}
