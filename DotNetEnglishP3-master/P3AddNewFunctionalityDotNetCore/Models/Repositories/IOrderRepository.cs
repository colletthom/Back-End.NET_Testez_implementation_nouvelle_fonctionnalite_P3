using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Models.Repositories
{
    public interface IOrderRepository
    {
        void Save(Order order);
        Task<Order> GetOrder(int? id);
        Task<IList<Order>> GetOrders();
    }
}
