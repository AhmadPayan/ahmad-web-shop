namespace WebShop.Application.Models
{
    public class OrderRequestDto
    {
        public required List<OrderItemDto> OrderItems { get; set; }
        public required string OrderReferenceId { get; set; }
    }
}
