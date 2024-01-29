using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Models.Services
{
    public interface IOrderService
    {
        void SaveOrder(OrderViewModel order);
        Task<Order> GetOrder(int id);
        Task<IList<Order>> GetOrders();
    }
}
