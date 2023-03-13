using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [Required, Range(1, 30)]
        public int Quantity { get; set; }
        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public ICollection<ProductCartItem> ProductCartItems { get; set; }
        
    }
}
