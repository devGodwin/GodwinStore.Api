using System.ComponentModel.DataAnnotations;

namespace GodwinStore.Api.Data.StoreData;


public class Customer
{ 
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; }
    public string Contact { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
}
