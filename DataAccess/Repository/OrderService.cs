using Data;
using Domain.Models;
using Service.OrderContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderService : IOrderService
    {
        private readonly ProductRepository _productRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly DataContext _dataContext;
        public OrderService(ProductRepository productRepository, CustomerRepository customerRepository, DataContext dataContext)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _dataContext = dataContext;
        }

        public Task<Order> AddAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<Order> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Order>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
