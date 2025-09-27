using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        Order AddOrder(Order Order);
        Order UpdateOrder(Order Order);
        bool DeleteOrder(int id);
    }
}
