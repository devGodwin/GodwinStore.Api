using GodwinStore.Api.Data.StoreData;

namespace GodwinStore.Api.Services.CustomerServices;

public interface ICustomerServices
{
    Task<bool> RegisterCustomerAsync(Customer customer);
    Task<Customer> GetCustomerByEmailAsync(string email);
    Task<Customer> LoginCustomerAsync(string email);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(string id);
    Task<bool> UpdateCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(Customer customer);
}