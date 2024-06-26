using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Models;

namespace WebShop.Application.Services
{
    public interface IOrderService
    {
        Task<Guid> PlaceOrderAsync(OrderRequestDto orderRequest);
    }
}
