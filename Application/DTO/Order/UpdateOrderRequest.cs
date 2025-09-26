﻿using Domain.Enums;
using Domain.ValueObjects;

namespace Application.DTO.Order
{
    public class UpdateOrderRequest
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<int> ListOfGames { get; set; } = new List<int>();
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentMethodDetails? PaymentMethodDetails { get; set; }
    }
}
