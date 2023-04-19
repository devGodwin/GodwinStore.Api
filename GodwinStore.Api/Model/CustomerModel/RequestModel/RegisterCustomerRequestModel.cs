using System.ComponentModel.DataAnnotations;

namespace GodwinStore.Api.Model.CustomerModel.RequestModel;

public class RegisterCustomerRequestModel
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Contact { get; set; }
    [Required,EmailAddress]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
    [Required, Compare("Password")]
    public string ConfirmPassword { get; set; }
}