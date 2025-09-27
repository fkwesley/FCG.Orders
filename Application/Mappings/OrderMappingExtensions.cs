using Application.DTO.Order;
using Domain.Entities;
using Application.Helpers;
using Domain.Enums;
using Application.DTO.Game;

namespace Application.Mappings
{
    public static class OrderMappingExtensions
    {
        /// <summary>
        /// Maps a AddOrderRequest to a Order entity.
        public static Order ToEntity(this AddOrderRequest request)
        {
            return new Order
            {
                PaymentMethod = request.PaymentMethod,
                Status = OrderStatus.PendingPayment,
                UserId = request.UserId.ToUpper(),
                PaymentMethodDetails = request.PaymentMethodDetails,
                ListOfGames = request.ListOfGames.Select(id => new Game { GameId = id }).ToList()
            };
        }

        /// <summary>
        /// Maps a UpdateOrderRequest to a Order entity.
        public static Order ToEntity(this UpdateOrderRequest request)
        {
            return new Order
            {
                OrderId = request.OrderId,
                PaymentMethod = request.PaymentMethod,
                Status = OrderStatus.PendingPayment,
                UserId = request.UserId.ToUpper(),
                PaymentMethodDetails = request.PaymentMethodDetails,
                ListOfGames = request.ListOfGames.Select(id => new Game { GameId = id }).ToList()
            };
        }

        /// <summary>
        /// Maps a Order entity to a OrderResponse.
        public static OrderResponse ToResponse(this Order entity)
        {
            return new OrderResponse
            {
                OrderId = entity.OrderId,
                UserId = entity.UserId.ToUpper(),
                PaymentMethod = entity.PaymentMethod,
                Status = entity.Status,
                ListOfGames = entity.ListOfGames.Select(game => new GameResponse
                {
                    GameId = game.GameId,
                    Name = game.Name,
                    Price = game.Price
                }).ToList(),
                TotalPrice = entity.TotalPrice,
                CreatedAt = DateTimeHelper.ConvertUtcToTimeZone(entity.CreatedAt, "E. South America Standard Time"),
                UpdatedAt = entity.UpdatedAt.HasValue ?
                                DateTimeHelper.ConvertUtcToTimeZone(entity.UpdatedAt.Value, "E. South America Standard Time") : (DateTime?)null,
            };
        }
    }
}
