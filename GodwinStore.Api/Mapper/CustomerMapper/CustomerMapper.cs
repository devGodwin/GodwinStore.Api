using AutoMapper;
using GodwinStore.Api.Data;
using GodwinStore.Api.Data.StoreData;
using GodwinStore.Api.Model.CustomerModel.RequestModel;
using GodwinStore.Api.Model.CustomerModel.ResponseModel;

namespace GodwinStore.Api.Mapper.CustomerMapper;

public class CustomerMapper:Profile
{
    public CustomerMapper()
    {
        CreateMap<Customer, RegisterCustomerRequestModel>().ReverseMap();
        CreateMap<Customer, RegisterCustomerUpdateModel>().ReverseMap();
        CreateMap<Customer, CustomerResponseModel>().ReverseMap();
    }
}