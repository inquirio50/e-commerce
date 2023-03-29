﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.ProductDTO
{
    public class ProductOrderGet
    {
        [Key]
        public int ProductId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50)]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required, DataType(DataType.ImageUrl)]
        public string Image { get; set; }
    }
}
