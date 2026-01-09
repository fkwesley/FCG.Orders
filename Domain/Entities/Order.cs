using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using System.Diagnostics;
using System.Globalization;

namespace Domain.Entities
{
    public class Order
    {
        [DebuggerDisplay("OrderId: {OrderId}, UserId: {UserId}, ListOfGames: {ListOfGames.Count}, Status: {Status}")]
        public int OrderId { get; set; }
        public required string UserId { get; set; }
        public required string UserEmail { get; set; }
        public ICollection<Game> ListOfGames { get; set; } = new List<Game>(); // Propriedade de navegação para os jogos selecionados
        public required OrderStatus Status
        {
            get => _status;
            set
            {
                if (_status == OrderStatus.Released)
                    throw new BusinessException("Cannot change the status of an order that is already released.");

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
                
                if (value != null)
                {
                    if (!IsValidCardNumber(value.Value.CardNumber))
                        throw new BusinessException("Invalid card number.");
                    
                    if (!IsValidExpiryDate(value.Value.ExpiryDate))
                        throw new BusinessException("The card has already expired or is invalid. Provide a new card");
                }

                _paymentMethodDetails = value;
            }
        }


        public bool IsValidCardNumber(string cardNumber)
        {
            return !string.IsNullOrWhiteSpace(cardNumber) && cardNumber.Length >= 13 && cardNumber.Length <= 19;
        }

        public bool IsValidExpiryDate(string expiryDate)
        {
            //returns true if the expiry date is in the future
            if (DateTime.TryParseExact(expiryDate, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Considera o último dia do mês como válido
                var lastDayOfMonth = new DateTime(parsedDate.Year, parsedDate.Month, DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month));
                return lastDayOfMonth >= DateTime.UtcNow.Date;
            }
            else
                return false;
        }

    }
}
