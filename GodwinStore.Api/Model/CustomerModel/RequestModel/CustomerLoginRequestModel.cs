using System.ComponentModel.DataAnnotations;

namespace GodwinStore.Api.Model.CustomerModel.RequestModel;

public class CustomerLoginRequestModel
{
    [Required,EmailAddress]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } 
    
}