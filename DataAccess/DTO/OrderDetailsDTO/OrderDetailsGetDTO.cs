using DataAccess.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.OrderDetailsDTO
{
    public class OrderDetailsGetDTO
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public ProductOrderGet Product { get; set; }
    }
}
