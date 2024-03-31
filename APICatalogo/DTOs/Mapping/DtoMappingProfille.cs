using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mapping
{
    public class DtoMappingProfille : Profile
    {
        public DtoMappingProfille()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}