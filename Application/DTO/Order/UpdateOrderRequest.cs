using Domain.Enums;
using Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace Application.DTO.Order
{
    public class UpdateOrderRequest
    {
        [JsonIgnore]
        public required int OrderId { get; set; }
        public required string UserId { get; set; }
        public IEnumerable<int> ListOfGames { get; set; } = new List<int>();
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentMethodDetails? PaymentMethodDetails { get; set; }
    }
}
