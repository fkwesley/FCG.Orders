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
        public required OrderStatus Status
        {
            get => _status;
            set
            {
                if (_status == OrderStatus.Paid)
                    throw new BusinessException("Cannot change the status of an order that is already paid.");

                _status = value;
            }
        }
        private OrderStatus _status;

        public required PaymentMethod PaymentMethod { get; set; }
        private PaymentMethodDetails? _paymentMethodDetails { get; set; } = null; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public double TotalPrice 
        { 
            get 
            { 
                if (ListOfGames == null || !ListOfGames.Any()) 
                    return 0; 
                
                return ListOfGames.Sum(game => game.Price); 
            }
        }

        // Regra de negócio para exigir detalhes do método de pagamento se for cartão de crédito
        public PaymentMethodDetails? PaymentMethodDetails
        {
            get => _paymentMethodDetails;
            set
            { 
                if (PaymentMethod != PaymentMethod.Pix && value == null)
                    throw new BusinessException("Payment method details are required for credit or debit card payments.");

                _paymentMethodDetails = value;
            }
        }

    }
}
