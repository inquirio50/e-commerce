using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime CreateAt { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime UpdateAt { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
