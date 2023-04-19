using AutoMapper;
using GodwinStore.Api.Data.StoreData;
using GodwinStore.Api.Model.OrderModel.RequestModel;
using GodwinStore.Api.Model.OrderModel.ResponseModel;

namespace GodwinStore.Api.Mapper.OrderMapper;

public class OrderMapper:Profile
{
    public OrderMapper()
    {
        CreateMap<Data.StoreData.Order, OrderRequestModel>().ReverseMap();
        CreateMap<Data.StoreData.Order, OrderUpdateModel>().ReverseMap();
        CreateMap<Data.StoreData.Order, OrderResponseModel>().ReverseMap();
    }
}