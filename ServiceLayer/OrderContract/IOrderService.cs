using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.OrderContract
{
    public interface IOrderService
    {
        Task<Order> AddAsync(Order order);
        Task<Order> GetByIdAsync(int id);
        Task<IQueryable<Order>> GetAllAsync();
        Task<Order> UpdateAsync(Order order);
        Task<Order> DeleteAsync(int id);
    }
}
