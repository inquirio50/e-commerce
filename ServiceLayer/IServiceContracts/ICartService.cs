using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServiceContracts
{
    public interface ICartService
    {
        Task<Cart> CreateCart(Cart cart);
        Task<Cart> UpdateCart(Cart cart);
        Task<bool> DeleteCart(int cartId, int customerId);
        Task<Cart> GetCartById(int cartId);
        Task<bool> AddProductToCart(Product product, int cartId, int customerId);
        Task<bool> RemoveProductFromCart(int cartId, int productId);
        Task<ICollection<Product>> GetCartProducts(int customerId, int cartId);
        Task<int> GetProductCount(int customerId, int cartId);
        Task<Product> GetCartProductById(int productId, int cardId, int customerId);
        Task<int> GetTotalPrice(int customerId, int cartId);
    }
}
