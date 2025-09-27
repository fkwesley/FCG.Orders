using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using System.Diagnostics;

namespace Domain.Entities
{
    public class Order
    {
        [DebuggerDisplay("OrderId: {OrderId}, UserId: {UserId}, ListOfGames: {ListOfGames.Count}, Status: {Status}")]
        public int OrderId { get; set; }
        public required string UserId { get; set; }
        public ICollection<Game> ListOfGames { get; set; } = new List<Game>(); // Propriedade de navegação para os jogos selecionados
        public required OrderStatus Status { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public PaymentMethodDetails? PaymentMethodDetails { get; set; } = null; 

        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public double TotalPrice 
        { 
            get 
            { 
                if (ListOfGames == null || !ListOfGames.Any()) 
                    return 0; 
                
                // Supondo que cada jogo tenha uma propriedade Price
                return ListOfGames.Sum(game => game.Price); 
            }
        }

        //regra de negócio para exigir detalhes do método de pagamento se for cartão de crédito
        public void ValidatePaymentDetails()
        {
            if (PaymentMethod != PaymentMethod.Pix && PaymentMethodDetails == null)
                throw new BusinessException("Payment method details are required for credit or debit card payments.");
        }

    }
}
