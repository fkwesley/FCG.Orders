using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Domain.ValueObjects
{
    public struct PaymentMethodDetails
    {
        [RegularExpression(@"^\d{4}-\d{4}-\d{4}-\d{4}$", ErrorMessage = "CardNumber must be in the format 0123-4567-8901-2345")]
        [DisplayFormat(DataFormatString = "0123-4567-8901-2345")]
        public required string CardNumber { get; init; }
        public required string CardHolder { get; init; }

        [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "ExpiryDate must be in the format yyyy-MM")]
        [DisplayFormat(DataFormatString = "yyyy-MM")]
        public required string ExpiryDate { get; set; }

        [RegularExpression(@"^\d{3}$", ErrorMessage = "Cvv must be 3 digits")]
        [DisplayFormat(DataFormatString = "123")]
        public required string Cvv { get; init; }

        public PaymentMethodDetails(string cardNumber, string cardHolder, string expiryDate, string cvv)
        {
            if (!IsValidCardNumber(cardNumber))
                throw new BusinessException("Invalid card number.");

            if (!IsValidExpiryDate(expiryDate))
                throw new BusinessException("The card has already expired. Provide a new card");

            CardNumber = cardNumber;
            CardHolder = cardHolder;
            ExpiryDate = expiryDate;
            Cvv = cvv;
        }

        private static bool IsValidCardNumber(string cardNumber)
        {
            return !string.IsNullOrWhiteSpace(cardNumber) && cardNumber.Length >= 13 && cardNumber.Length <= 19;
        }

        private static bool IsValidExpiryDate(string expiryDate)
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