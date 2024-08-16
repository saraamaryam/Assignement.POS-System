using AutoMapper;
using POS.API.Models.DTO;
using POS.API.Models.Entities;

namespace POS.API.Data
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            CreateMap<SaleProducts, ProductsDTO>();

            CreateMap<FinalReceipt, FinalReceiptDTO>()
             .ForMember(dest => dest.Receipt, opt => opt.MapFrom(src => src.Receipt))
             .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            CreateMap<Receipt, ReceiptDTO>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
            CreateMap<LoginDTO, User>()
             .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
             .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password));

            CreateMap<RegisterDTO, User>()
             .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
             .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.role));

            CreateMap<User, LoginDTO>()
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password))
            .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.role));
            CreateMap<UserRoleDTO, UserRole>();

            CreateMap<User, UserRoleDTO>()
            .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.role));

            CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
             .ForMember(dest => dest.category, opt => opt.MapFrom(src => src.Category))
             .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.type, opt => opt.MapFrom(src => src.Type));

            CreateMap<UpdateProductDTO, Product>()
          .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Price))
           .ForMember(dest => dest.category, opt => opt.MapFrom(src => src.Category))
           .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.Quantity))
          .ForMember(dest => dest.type, opt => opt.MapFrom(src => src.Type));

            CreateMap<User, RegisterDTO>();
        }
    }
}
