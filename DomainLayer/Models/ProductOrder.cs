using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProductOrder
    {
        [Key]
        public int ProductOrderId { get; set; }
        [ForeignKey(nameof(OrderedProduct))]
        public int ProductId { get; set; }
        public Product OrderedProduct { get; set; }
        [ForeignKey(nameof(OrderPlaced))]
        public int OrderId { get; set; }
        public Order OrderPlaced { get; set; }
    }
}
