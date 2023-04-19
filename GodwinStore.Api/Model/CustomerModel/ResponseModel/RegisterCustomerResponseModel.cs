using System.ComponentModel.DataAnnotations;

namespace GodwinStore.Api.Model.CustomerModel.ResponseModel;

public class CustomerResponseModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; }
    public string Contact { get; set; }
    public string Email { get; set; }
}