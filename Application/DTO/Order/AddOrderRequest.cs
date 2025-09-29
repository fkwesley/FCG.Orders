using Domain.Enums;
using Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace Application.DTO.Order
{
    public class AddOrderRequest
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public IEnumerable<int> ListOfGames { get; set; } = new List<int>();
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentMethodDetails? PaymentMethodDetails { get; set; }
    }
}
