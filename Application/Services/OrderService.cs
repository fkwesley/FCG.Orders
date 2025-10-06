using Application.DTO.Order;
using Application.Exceptions;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.ValueObjects;
using FCG.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILoggerService _loggerService;
        private readonly IGameService _gameService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public OrderService(
                IOrderRepository orderRepository, 
                ILoggerService loggerService,
                IGameService gameService,
                IHttpContextAccessor httpContext,
                IServiceScopeFactory scopeFactory,
                IServiceBusPublisher serviceBusPublisher)
        {
            _orderRepository = orderRepository
                ?? throw new ArgumentNullException(nameof(orderRepository));
            _loggerService = loggerService;
            _gameService = gameService;
            _httpContext = httpContext;
            _scopeFactory = scopeFactory;
            _serviceBusPublisher = serviceBusPublisher;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = _orderRepository.GetAllOrders();

            using var scope = _scopeFactory.CreateScope();
            var loggerService = scope.ServiceProvider.GetRequiredService<ILoggerService>();

            await loggerService.LogTraceAsync(new Trace
            {
                LogId = _httpContext.HttpContext?.Items["RequestId"] as Guid?,
                Timestamp = DateTime.UtcNow,
                Level = LogLevel.Info,
                Message = "Retrieved all Orders",
                StackTrace = null
            });

            return orders.Select(order => order.ToResponse()).ToList();
        }

        public OrderResponse GetOrderById(int id)
        {
            var orderFound = _orderRepository.GetOrderById(id);

            return orderFound.ToResponse();
        }

        public OrderResponse AddOrder(AddOrderRequest order)
        {
            //getting user orders
            var userOrders = _orderRepository.GetAllOrders().Where(o => o.UserId.Equals(order.UserId, StringComparison.OrdinalIgnoreCase));

            //verifying if there is any active order with some of the games requested
            if (userOrders.Any(o => o.ListOfGames.Any(g => order.ListOfGames.Contains(g.GameId))
                                 && o.Status != OrderStatus.Cancelled 
                                 && o.Status != OrderStatus.Released
                                 && o.Status != OrderStatus.Refunded))
                throw new ValidationException(string.Format("There is already an active order for the user {0} with one or more of the games requested.", order.UserId.ToUpper()));

            var orderEntity = order.ToEntity();

            //verifying if all games exists and getting their prices
            foreach (var game in orderEntity.ListOfGames)
            {
                var existingGame = _gameService.GetGameById(game.GameId);

                if (existingGame == null)
                    throw new ValidationException(string.Format("Game with id {0} is not available.", game.GameId));

                game.Price = existingGame.Price;
                game.Name = existingGame.Name;
            }
            
            var orderAdded = _orderRepository.AddOrder(orderEntity);

            // Publishing payment event to the queue on Azure Service Bus
            _serviceBusPublisher.PublishMessageAsync(
                topicName: "fcg.paymentstopic", 
                message: new 
                {
                    orderAdded.OrderId,   
                    amount = orderAdded.TotalPrice,
                    orderAdded.PaymentMethod,
                    order.PaymentMethodDetails?.CardNumber,
                    order.PaymentMethodDetails?.CardHolder,
                    order.PaymentMethodDetails?.ExpiryDate,
                    order.PaymentMethodDetails?.Cvv
                }, 
                customProperties: new Dictionary<string, object>
                {
                    {"PaymentMethod", orderAdded.PaymentMethod.ToString() }
                });

            return orderAdded.ToResponse();
        }

        public OrderResponse UpdateOrder(UpdateOrderRequest order)
        {
            var orderEntity = order.ToEntity();
            var orderUpdated = _orderRepository.UpdateOrder(orderEntity);

            return orderUpdated.ToResponse();
        }

        public bool DeleteOrder(int id)
        {
            return _orderRepository.DeleteOrder(id);
        }

    }
}
