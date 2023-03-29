using Data;
using Domain.Contract;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IBaseContract<Order>, IBaseContract2<OrderDetails>
    {
        private readonly DataContext _orderContext;
        public OrderRepository(DataContext orderContext)
        {
            _orderContext = orderContext;
        }
        public async Task<Order> AddAsync(Order order)
        {
            try
            {
                if (order != null)
                {
                    await _orderContext.Orders.AddAsync(order);
                    await _orderContext.SaveChangesAsync();
                    return order;
                }
                else
                {
                    throw new ArgumentException("The order cannot be null or empty.", nameof(order));
                }
            }

            catch (DbUpdateException ex)
            {
                throw new DbUpdateException("An error occurred while adding this order.", ex);
            }
        }

        public async Task<OrderDetails> AddNestedAsync(OrderDetails details, int id)
        {
            try
            {
                var orderDetails = await _orderContext.Orders.Include(r => r.OrderDetails).FirstOrDefaultAsync(x => x.OrderId== id);
                if (orderDetails != null)
                {
                    orderDetails.OrderDetails.Add(details);
                    await _orderContext.SaveChangesAsync();
                    return details;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new Exception("An error has occured");
            }
        }

        public async Task<IQueryable<Order>> GetAllAsync()
        {
            try
            {
                var getOrders = await _orderContext.Orders.ToListAsync();
                return getOrders.AsQueryable();
            }

            catch
            {
                throw new Exception("An error occurred while retrieving the Orders.");
            }
        }

        public async Task<IQueryable<OrderDetails>> GetAllNestedAsync(int id)
        {
            try
            {
                var check = _orderContext.OrderDetails.FirstOrDefault(p => p.OrderDetailId == id);
                if (check != null)
                {
                    var orderDetails = await _orderContext.OrderDetails.ToListAsync();
                    return orderDetails.AsQueryable();
                }
                else
                {
                    throw new Exception("not found");
                }

            }
            catch
            {
                throw new Exception("An error occured while trying to retrieve the Order Details for this order");
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderContext.Orders.FirstOrDefaultAsync(r => r.OrderId == id);
                if (order != null)
                {
                    return order;
                }
                else
                {
                    throw new Exception($"Order with ID {id} was not found.");

                }
            }
            catch
            {
                throw new Exception("An error occurred while retrieving this Order.");
            }
        }

        public async Task<IEnumerable<Order>> GetFiltered(Expression<Func<Order, bool>> filtered)
        {
            try
            {
                var filter = await _orderContext.Orders.Where(filtered).ToListAsync();
                if (filter != null)
                {
                    return filter;
                }
                else
                {
                    throw new Exception("You have not specified any filter condition");
                }
            }
            catch
            {
                throw new Exception("An error occured while tryin to retrieve the filtered orders");
            }
        }

        public async Task<OrderDetails> GetNestedByIdAsync(int orderId, int orderDetailsId)
        {
            try
            {
                var details = await _orderContext.OrderDetails.FirstOrDefaultAsync(x => x.OrderDetailId == orderDetailsId && x.OrderId == orderId);
                if (details != null)
                {
                    return details;
                }
                else
                {
                    return null;
                }
            }

            catch
            {
                throw new Exception("An error occurred while retrieving the order.");
            }
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            try
            {
                var orderCheck = await _orderContext.Orders.FindAsync(order.OrderId);
                if (orderCheck != null)
                {
                    _orderContext.Orders.Update(orderCheck);
                    await _orderContext.SaveChangesAsync();
                    return orderCheck;
                }
                else
                {
                    throw new Exception($"Order doesn't exist");
                }

            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException("An error occurred while updating this order.", ex);
            }
        }

        public async Task<OrderDetails> UpdateNestedAsync(OrderDetails details, int orderId)
        {
            try
            {
                var idCheck = await _orderContext.Orders.Include(r => r.OrderDetails).FirstOrDefaultAsync(x => x.OrderId == orderId);
                if (idCheck != null)
                {
                    idCheck.OrderDetails.Add(details);
                    await _orderContext.SaveChangesAsync();
                    return details;
                }
                else
                {
                    return null;
                }
            }

            catch
            {
                throw new DbUpdateException("An error occurred while updating the OrderDetails.");
            }
        }

        public async Task<Order> DeleteAsync(int orderId)
        {
            try
            {
                var delete = await _orderContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
                if (delete != null)
                {
                    _orderContext.Orders.Remove(delete);
                    await _orderContext.SaveChangesAsync();
                    return delete;

                }
                else
                {
                    throw new Exception($"Order with ID {orderId} was not found.");

                }
            }
            catch
            {
                throw new Exception("An error occurred while deleting this Order.");
            }
        }

        public async Task<OrderDetails> DeleteNestedAsync(int orderId, int orderDetailsId)
        {
            try
            {
                var idCheck = await _orderContext.OrderDetails.FirstOrDefaultAsync(x => x.OrderDetailId == orderDetailsId);
                if (idCheck != null)
                {
                    _orderContext.Remove(idCheck);
                    await _orderContext.SaveChangesAsync();
                    return idCheck;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new Exception("An error occured while deleting this orderdetail");
            }
        }
    }
}
