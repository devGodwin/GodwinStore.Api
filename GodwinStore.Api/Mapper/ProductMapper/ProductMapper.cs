using AutoMapper;
using GodwinStore.Api.Data;
using GodwinStore.Api.Model.ProductModel.RequestModel;
using GodwinStore.Api.Model.ProductModel.ResponseModel;

namespace GodwinStore.Api.Mapper.ProductMapper;

public class ProductMapper:Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductRequestModel>().ReverseMap();
        CreateMap<Product, ProductUpdateModel>().ReverseMap();
        CreateMap<Product, ProductResponseModel>().ReverseMap();
    }
}