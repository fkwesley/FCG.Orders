using Application.DTO.Game;
using Application.DTO.Order;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.UnitTests.Application.Mappings
{
    public class GameMappingExtensionsTests
    {
        [Fact]
        public void ToEntity_ShouldMapAllPropertiesCorrectly()
        {
            // Arrange
            var paymentDetails = new PaymentMethodDetails
            {
                CardNumber = "1234567890123456",
                CardHolder = "John Doe",
                ExpiryDate = "2026-08",
                Cvv = "123"
            };

            var request = new AddOrderRequest
            {
                UserId = "CARL.JOHNSON",
                ListOfGames = new List<int> { 1, 2 },
                PaymentMethod = PaymentMethod.CreditCard,
                PaymentMethodDetails = paymentDetails
            };

            // Act
            var result = request.ToEntity();

            // Assert
            result.UserId.Should().Be(request.UserId);
            result.PaymentMethod.Should().Be(request.PaymentMethod);
            result.PaymentMethodDetails.Should().Be(request.PaymentMethodDetails);
            result.Status.Should().Be(OrderStatus.PendingPayment);
            result.ListOfGames.Should().NotBeNull();
        }

        [Fact]
        public void ToResponse_ShouldMapAllPropertiesCorrectly()
        {
            // Arrange
            var games = new List<Game>
                {
                    new Game { GameId = 1, Price = 100 },
                    new Game { GameId = 2, Price = 150 }
                };

            var paymentDetails = new PaymentMethodDetails
            {
                CardNumber = "1234567890123456",
                CardHolder = "John Doe",
                ExpiryDate = "2026-08",
                Cvv = "123"
            };

            var entity = new Order
            {
                OrderId = 99,
                UserId = "MAX.PAYNE",
                ListOfGames = games,
                PaymentMethod = PaymentMethod.CreditCard,
                PaymentMethodDetails = paymentDetails,
                Status = OrderStatus.Paid,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            // Act
            var result = entity.ToResponse();

            // Assert
            result.OrderId.Should().Be(entity.OrderId);
            result.UserId.Should().Be(entity.UserId);
            result.PaymentMethod.Should().Be(entity.PaymentMethod);
            result.Status.Should().Be(entity.Status);
            result.ListOfGames.Should().AllBeOfType<GameResponse>();
            result.TotalPrice.Should().Be(entity.TotalPrice);
            result.CreatedAt.Kind.Should().Be(DateTimeKind.Local);
            result.UpdatedAt.Should().BeNull();
        }
    }
}
