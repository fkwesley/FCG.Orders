using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public struct PaymentMethodDetails
    {
        public required string CardNumber { get; init; }
        public required string CardHolder { get; init; }
        public required DateTime ExpiryDate { get; init; }
        public required string Cvv { get; init; }

        public PaymentMethodDetails(string cardNumber, string cardHolder, DateTime expiryDate, string cvv)
        {
            if (!IsValidCardNumber(cardNumber))
                throw new BusinessException("Invalid card number.");

            CardNumber = cardNumber;
            CardHolder = cardHolder;
            ExpiryDate = expiryDate;
            Cvv = cvv;
        }

        private static bool IsValidCardNumber(string cardNumber)
        {
            //validação de padrão de cartão de crédito
            return !string.IsNullOrWhiteSpace(cardNumber) && cardNumber.Length >= 13 && cardNumber.Length <= 19;
        }
    }
}