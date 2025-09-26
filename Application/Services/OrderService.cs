using Application.DTO.Order;
using Application.Exceptions;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILoggerService _loggerService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderService(
                IOrderRepository orderRepository, 
                ILoggerService loggerService,
                IHttpContextAccessor httpContext,
                IServiceScopeFactory scopeFactory)
        {
            _orderRepository = orderRepository
                ?? throw new ArgumentNullException(nameof(orderRepository));
            _loggerService = loggerService;
            _httpContext = httpContext;
            _scopeFactory = scopeFactory;
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
            //verify if order already exists for the same user with the same games and status is not cancelled
            if (_orderRepository.GetAllOrders().Any(o => o.UserId == order.UserId
                                                      && o.ListOfGames.SequenceEqual((IEnumerable<Game>)order.ListOfGames) 
                                                      && o.Status != OrderStatus.Cancelled))
                throw new ValidationException(string.Format("Order for user {0} with the same games already exists.",order.UserId));

            var orderEntity = order.ToEntity();
            var orderAdded = _orderRepository.AddOrder(orderEntity);

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
