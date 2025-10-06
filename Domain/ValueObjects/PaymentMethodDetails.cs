using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Domain.ValueObjects
{
    public struct PaymentMethodDetails
    {
        [RegularExpression(@"^\d{4}-\d{4}-\d{4}-\d{4}$", ErrorMessage = "CardNumber must be in the format 0123-4567-8901-2345")]
        [DisplayFormat(DataFormatString = "0123-4567-8901-2345")]
        public required string CardNumber { get; set; }
        public required string CardHolder { get; set; }

        [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "ExpiryDate must be in the format yyyy-MM")]
        [DisplayFormat(DataFormatString = "yyyy-MM")]
        public required string ExpiryDate { get; set; }

        [RegularExpression(@"^\d{3}$", ErrorMessage = "Cvv must be 3 digits")]
        [DisplayFormat(DataFormatString = "123")]
        public required string Cvv { get; set; }

    }
}