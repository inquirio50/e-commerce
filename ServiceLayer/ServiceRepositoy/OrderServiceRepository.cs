using Data;
using DataAccess.DTO.OrdersDTO;
using DataAccess.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Service.OrderContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceRepositoy
{
    public class OrderServiceRepository : IOrderService
    {   private readonly CartServiceRepository _cartServiceRepository;
        private readonly CustomerRepository _customer;
        //private readonly OrderDetailsRepository _orderDetailsRepository;
        private readonly ProductRepository _product;
        private readonly DataContext _context;
        public OrderServiceRepository(DataContext context, CartServiceRepository cartServiceRepository, CustomerRepository customer, OrderDetailsRepository orderDetailsRepository, ProductRepository product) 
        {
            _cartServiceRepository = cartServiceRepository;
            _customer = customer;
            //_orderDetailsRepository = orderDetailsRepository;
            _product = product;
        }

        public async Task<bool> PlaceOrder(Order order, int customerId, int cartId)
        {
            try
            {
                //Check to see if customer exists
                var customer = await _customer.GetByIdAsync(customerId);
                if (customer == null)
                {
                    return false;
                }
                else
                {
                    //get cart
                    var cart = await _cartServiceRepository.GetCartById(cartId);
                    //check if cart is null
                    if (cart == null)
                    {
                        return false;
                    }
                    else
                    {
                        //loop through each product in the cart's product
                        foreach (var item in cart.Products)
                        {//check if the product is null
                            var product = await _product.GetByIdAsync(item.ProductId);
                            if (product == null)
                            {
                                return false;
                            }
                            //check to see if items in the cart is not more than the available product
                            else if (item.Quantity > product.Quantity)
                            {
                                return false;
                            }
                            else
                            {
                                //assign customerId in the customer to the customerId in the order class
                                order.CustomerId = customer.CustomerId;
                                order.OrderDate = DateTime.Now;
                                // create orerdetails in the order entitiy
                                var details = new OrderDetails
                                {
                                    Quantity = item.Quantity,
                                    ProductId = item.ProductId,
                                    OrderId = order.OrderId,
                                    CustomerName = customer.FirstName + " " + customer.LastName,
                                    CustomerPhoneNumber = customer.PhoneNumber,
                                    DeliveryAddress = customer.Address
                                };
                                //add details to order
                                order.OrderDetails.Add(details);
                            }
                        }
                    }   
                }
                //add order into Orders table
                await _context.Orders.AddAsync(order);
                //save changes
                await _context.SaveChangesAsync();
                //delete cart
                await _cartServiceRepository.DeleteCart(customerId, cartId);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CancelOrder(int customerId, int orderId)
        {
            try
            {
                var customer = await _customer.GetByIdAsync(customerId);
                if (customer == null)
                {
                    return false;
                }
                else
                {
                    var order = customer.Orders.FirstOrDefault(c => c.OrderId == orderId);
                    if(order == null)
                    {
                        return false;
                    }
                    else
                    {
                        customer.Orders.Remove(order);
                        await _context.SaveChangesAsync();
                    }
                    return true;
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ICollection<Order>> GetAllOrders()
        {
            try
            {
                var orders = await _context.Orders.ToListAsync();
                return orders;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ICollection<Order>> GetAllCustomerOrders(int customerId)
        {
            try
            {
                var customer = await _customer.GetByIdAsync(customerId);
                if(customer == null)
                {
                    return null;
                }
                else
                {
                    var order = customer.Orders.ToList();
                    return order;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Order> GetOrderById(int customerId, int orderId)
        {
            try
            {
                var customer = await _customer.GetByIdAsync(customerId);
                if (customer == null)
                {
                    return null;
                }
                else
                {
                    var order = await _context.Orders.FirstOrDefaultAsync(c => c.OrderId == orderId);
                    if(order == null)
                    {
                        return null;
                    }
                    else
                    {
                        return order;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<int> GetTotalPrice(int customerId, int orderId, int cartId)
        {
            try
            {
                var customer = await _customer.GetByIdAsync(customerId);
               if(customer == null)
                {
                    return 0;
                }
                else
                {
                    var order = customer.Orders.FirstOrDefault(o => o.OrderId==orderId);
                    if (order == null)
                    {
                        return 0;
                    }
                    else
                    {
                        var price = await _cartServiceRepository.GetTotalPrice(customerId, cartId);
                        return price;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateOrder(Order order, int customerId, int orderId)
        {
            try
            {
                var customer = await _customer.GetByIdAsync(customerId);
                if(customer == null)
                {
                    return false;
                }
                else
                {
                    var ordercheck = customer.Orders.FirstOrDefault(o => o.OrderId == orderId);
                    if(ordercheck == null)
                    {
                        return false;
                    }
                    else
                    {
                        order.CustomerId = customerId;
                        order.OrderId = orderId;
                        _context.Orders.Update(order);
                        await _context.SaveChangesAsync();
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
