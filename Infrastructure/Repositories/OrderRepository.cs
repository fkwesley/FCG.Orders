using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _context;

        public OrderRepository(OrdersDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Order> GetAllOrders()
        {
           return _context.Orders
                            .Include(o => o.ListOfGames)
                            .ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders
                            .Include(o => o.ListOfGames)
                            .FirstOrDefault(o => o.OrderId == id)
                ?? throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

        public Order AddOrder(Order game)
        {
            _context.Orders.Add(game);
            _context.SaveChanges();
            return game;
        }

        public Order UpdateOrder(Order order)
        {
            var existingOrder = GetOrderById(order.OrderId);

            if (existingOrder != null) {
                existingOrder.Status = order.Status;
                existingOrder.PaymentMethod = order.PaymentMethod;
                existingOrder.PaymentMethodDetails = order.PaymentMethodDetails;
                
                _context.Orders.Update(existingOrder);
                _context.SaveChanges();
            }
            else
                throw new KeyNotFoundException($"Order with ID {order.OrderId} not found.");

            return existingOrder;
        }

        public bool DeleteOrder(int id)
        {
            var game = GetOrderById(id);

            if (game != null)
            {
                _context.Orders.Remove(game);
                _context.SaveChanges();
                return true;
            }
            else
                throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

    }
}
