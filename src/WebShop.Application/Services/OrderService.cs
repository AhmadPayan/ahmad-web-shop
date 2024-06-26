using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Models;
using WebShop.Domain.Entities;
using WebShop.Domain.Events;
using WebShop.Infrastructure.Publishers;
using WebShop.Infrastructure.Repositories;

namespace WebShop.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventPublisher _eventPublisher;

    public OrderService(IOrderRepository orderRepository, IEventPublisher eventPublisher)
    {
        _orderRepository = orderRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> PlaceOrderAsync(OrderRequestDto orderRequest)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CreatedDateTime = DateTime.UtcNow,
            OrderItems = orderRequest.OrderItems.Select(item => new OrderItem {ProductId = item.ProductId, Quantity = item.Quantity }).ToList(),
            OrderReferenceId = orderRequest.OrderReferenceId
        };

        await _orderRepository.AddOrderAsync(order);
        await _eventPublisher.PublishOrderPlacedEventAsync(new OrderPlacedEvent
        {
            OrderId = order.Id,
            CreatedDateTime = order.CreatedDateTime,
            OrderItems = order.OrderItems,
            OrderReferenceId = order.OrderReferenceId
        });

        return order.Id;
    }
}
