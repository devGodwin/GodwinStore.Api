using GodwinStore.Api.Data.DbContexts;
using GodwinStore.Api.Data.StoreData;
using GodwinStore.Api.Model.CustomerModel.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace GodwinStore.Api.Services.CustomerServices;

public class CustomerServices:ICustomerServices
{
    private readonly CustomerContext _customerContext;
    
    public CustomerServices(CustomerContext customerContext)
    {
        _customerContext = customerContext;
    }

    public async Task<bool> RegisterCustomerAsync(Customer customer)
    {
        await _customerContext.Customer.AddAsync(customer);
     var rows= await _customerContext.SaveChangesAsync();
     return rows > 0;
    }
    
    public async Task<Customer> GetCustomerByEmailAsync(string email)
    {
      return await _customerContext.Customer.FirstOrDefaultAsync(x=>x.Email.Equals(email));
     
    }

    public async Task<Customer> LoginCustomerAsync(string email)
    {
     return await _customerContext.Customer.FirstOrDefaultAsync(x=>x.Email.Equals(email));
   
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
      return await _customerContext.Customer.AsNoTracking().ToListAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(string id)
    {
      return await _customerContext.Customer.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
       _customerContext.Customer.Update(customer);
      var rows= await _customerContext.SaveChangesAsync();
      return rows > 0;
    }

    public async Task<bool> DeleteCustomerAsync(Customer customer)
    {
        _customerContext.Customer.Remove(customer);
       var rows = await _customerContext.SaveChangesAsync();
        return rows > 0;
    }
    
}