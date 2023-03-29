using AutoMapper;
using DataAccess.DTO.ProductDTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Automapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper() 
        {
            CreateMap<ProductCreateDTO, Product>().ReverseMap();
            CreateMap<Product, ProductGetDTO>().ReverseMap();
            CreateMap<Product, ProductOrderGet>().ReverseMap();
        }
    }
}
