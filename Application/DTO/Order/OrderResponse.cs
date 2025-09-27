using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.DTO.Order
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public required string UserId { get; set; }
        public IEnumerable<Game> ListOfGames { get; set; } = new List<Game>(); 
        public required OrderStatus Status { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public PaymentMethodDetails? PaymentMethodDetails { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public double TotalPrice { get; set; }
    }
}
