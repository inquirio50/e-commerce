using DataAccess.DTO.OrderDetailsDTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.OrdersDTO
{
    public class OrdersCreateDTO
    {
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public List<OrderDetailsCreateDTO> OrderDetails { get; set; }
    }

}
