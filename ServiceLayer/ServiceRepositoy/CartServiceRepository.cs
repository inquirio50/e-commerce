using Data;
using DataAccess.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Service.IServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceRepositoy
{
    public class CartServiceRepository : ICartService
    {
        private readonly DataContext _context;
        private readonly CustomerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        public CartServiceRepository(DataContext context, CustomerRepository customer, ProductRepository product) 
        {
            _context= context;
            _customerRepository= customer;
            _productRepository= product;
        }

        public async Task<Cart> CreateCart(Cart cart)
        {
            try
            {
                var customer = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == cart.CustomerId);
                if (customer == null)
                {
                    return null;
                }
                else if (cart != null)
                {
                    await _context.Carts.AddAsync(cart);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    return null;
                }
                return cart;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        public async Task<Cart> GetCartById(int cartId)
        {
            try
            {
                var cart = await _context.Carts.
                    Include(p => p.Products).
                    FirstOrDefaultAsync(c => c.CartId == cartId);
                if(cart ==null)
                {
                    return null;
                }
                else
                {
                    return cart;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Cart> UpdateCart(Cart cart)
        {
            try
            {
                var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cart.CartId && c.CustomerId == cart.CustomerId);

                if (existingCart == null)
                {
                    return null;
                }
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();

                return cart;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCart(int cartId, int customerId)
        {
            try
            {
                var validation = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == customerId || c.CartId == cartId);
                if (validation == null)
                {
                    return false;
                }
                else
                {
                    _context.Remove(validation);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddProductToCart(Product product, int cartId, int customerId)
        {
            try
            {
                //check if customer exists
                var customer = await _customerRepository.GetByIdAsync(customerId);
                if (customer == null)
                {
                    return false;
                }
                else
                {
                    //check if product exists
                    var products = await _productRepository.GetByIdAsync(product.ProductId);
                    if (products == null && product.IsAvailable == false)
                    {
                        return false;
                    }
                    else
                    {
                        //check Cart to see if product already exists
                        var cart = await _context.Carts.Include(c => c.Products).FirstOrDefaultAsync(c => c.CartId == cartId);
                        if (cart != null)
                        {
                            var existingProduct = cart.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                            if (existingProduct != null)
                            {
                                if (existingProduct.Quantity > products.Quantity)
                                {
                                    cart.Products.Remove(existingProduct);
                                    await _context.SaveChangesAsync();
                                    return false;
                                }
                                else
                                {
                                    existingProduct.Quantity++;
                                }
                            }
                            else
                            {
                                products.Quantity = 1;
                                cart.Products.Add(products);
                            }
                        }
                        else
                        {
                            // Create a new cart if there's no existing cart
                            cart = new Cart
                            {
                                CartId = cartId,
                                CustomerId = customerId,
                                Products = new List<Product> { products }
                            };
                            //await _context.Carts.AddAsync(cart);
                            //cart.Products = new List<Product> { products };
                            //cart.CartId = cartId;
                            //cart.CustomerId = customerId;
                            await _context.Carts.AddAsync(cart);
                        }
                        //check to see if the products in the cart are not more than the available products
                        foreach(var item in cart.Products)
                        {
                            if (item.Quantity > product.Quantity)
                            {
                                // Remove the product from the cart and return false
                                cart.Products.Remove(item);
                                await _context.SaveChangesAsync();
                                return false;
                            }
                            else
                            {
                                await _context.SaveChangesAsync();
                            }
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Product> GetCartProductById(int productId, int cartId, int customerId)
        {
            try
            {
                var cart = await GetCartById(cartId);
                if (cart != null)
                {
                    return cart.Products.FirstOrDefault(p => p.ProductId == productId);  
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetProductCount(int customerId, int cartId)
        {
            try
            {
                var cart = await GetCartById(cartId);
                var products = cart.Products.Count;
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<ICollection<Product>> GetCartProducts(int customerId, int cartId)
        {
            try
            {
                var cart = await GetCartById(cartId);
                if (cart != null)
                {
                    var product = cart.Products.ToList();
                    return product;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetTotalPrice(int cartId, int customerId)
        {
            //var cartProducts = await GetCartProducts(customerId, cartId);
            //int totalPrice = 0;

            //foreach (var cartProduct in cartProducts)
            //{
            //    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == cartProduct.Products.First().ProductId);
            //    if (product == null)
            //    {
            //        continue;
            //    }

            //    totalPrice += product.Price * cartProduct.Products.First().Quantity;
            //}

            //return totalPrice;

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.CartId == cartId && c.CustomerId == customerId);

                if (cart == null)
                {
                    return 0;
                }
                else
                {
                    int totalPrice = cart.Products.Sum(p => p.Price * p.Quantity);
                    return totalPrice;
                } 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveProductFromCart(int cartId, int productId)
        {
            try
            {
                var cart = await _context.Carts.Include(p => p.Products).FirstOrDefaultAsync(c => c.CartId == cartId);
                if(cart == null)
                {
                    return false;
                }
                else
                {
                    var product = cart.Products.FirstOrDefault(p => p.ProductId == productId);
                    if(product == null)
                    {
                        return false;
                    }
                    else
                    {
                        cart.Products.Remove(product);
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
