using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos.Category;
using AutoMapper;

namespace ApiEcommerce.Mapping;

public class CategoryProfile : Profile
{
  public CategoryProfile()
  {
    CreateMap<Category, CategoryDto>().ReverseMap();
    CreateMap<Category, CreateCategoryDto>().ReverseMap();
  }
}
