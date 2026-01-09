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
                UserEmail = request.Email,
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
                Status = request.Status,
                UserId = request.UserId.ToUpper(),
                UserEmail = string.Empty,         // Default value, as it's not provided in the update request
                PaymentMethod = PaymentMethod.Pix // Default value, as it's not provided in the update request
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
                UserEmail = entity.UserEmail.ToLower(),
                PaymentMethod = entity.PaymentMethod,
                Status = entity.Status,
                ListOfGames = entity.ListOfGames.Select(game => new GameResponse
                {
                    GameId = game.GameId,
                    Name = game.Name,
                    Price = game.Price
                }).ToList(),
                TotalPrice = entity.TotalPrice,
                CreatedAt = DateTimeHelper.ConvertUtcToTimeZone(entity.CreatedAt, "America/Sao_Paulo"),
                UpdatedAt = entity.UpdatedAt.HasValue ?
                                DateTimeHelper.ConvertUtcToTimeZone(entity.UpdatedAt.Value, "America/Sao_Paulo") : (DateTime?)null,
            };
        }
    }
}
