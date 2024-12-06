using AutoMapper;
using E_commerce.API.DTO;
using E_commerce.Core.DTO;
using E_commerce.Core.Models;

namespace e_commerce.ConfigurationAutoMapper
{
	public class AutoMapperConfig:Profile
	{
        public AutoMapperConfig()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<PaymentDTO, Payment>();
            CreateMap<OrderDTO, Order>();
        }
    }
}
