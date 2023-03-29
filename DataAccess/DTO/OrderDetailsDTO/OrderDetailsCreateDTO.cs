using DataAccess.DTO.ProductDTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.OrderDetailsDTO
{
    public class OrderDetailsCreateDTO
    {
        [Required]
        public int Quantity { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
    }
}
