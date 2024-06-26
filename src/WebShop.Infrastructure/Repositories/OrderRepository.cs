using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Entities;

namespace WebShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static readonly List<Order> _orders = new();

        public Task AddOrderAsync(Order order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }
    }
}
