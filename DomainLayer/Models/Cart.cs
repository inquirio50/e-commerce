using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DateTime CreateAt { get; set; } = DateTime.Now;
        [Required, DataType(DataType.DateTime)]
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
