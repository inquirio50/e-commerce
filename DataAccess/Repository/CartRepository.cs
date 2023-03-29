using Data;
using Domain.Contract;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CartRepository : IBaseContract<Cart>
    {
        private readonly DataContext _cartContext;
        public CartRepository() 
        {
        
        }
        public Task<Cart> AddAsync(Cart cart)
        {
            throw new NotImplementedException();
        }
        public Task<Cart> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IQueryable<Cart>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Cart> UpdateAsync(Cart cart)
        {
            throw new NotImplementedException();
        }
        public Task<Cart> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
