using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.DTOs.CategoryDto;
using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mapping
{
    public class DtoMappingProfille : Profile
    {
        public DtoMappingProfille()
        {
            CreateMap<Category, CategoryRequestDto>().ReverseMap();
            CreateMap<Category, CategoryResponseDto>().ReverseMap();

            CreateMap<Product, ProductRequestDto>().ReverseMap();
            CreateMap<Product, ProductResponseDto>().ReverseMap();
                
        }
    }
}