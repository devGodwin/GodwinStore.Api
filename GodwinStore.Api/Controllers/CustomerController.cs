using System.Net;
using System.Security.Cryptography;
using AutoMapper;
using GodwinStore.Api.Data.StoreData;
using GodwinStore.Api.Helper;
using GodwinStore.Api.Model.CustomerModel.RequestModel;
using GodwinStore.Api.Model.CustomerModel.ResponseModel;
using GodwinStore.Api.Services.CustomerServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GodwinStore.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController:ControllerBase
{
    private readonly ICustomerServices _customerServices;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerController> _logger;
   

    public CustomerController(ICustomerServices customerServices, IMapper mapper, ILogger<CustomerController> logger)
    {
        _customerServices = customerServices;
        _mapper = mapper;
        _logger = logger;
    }
    

    /// <summary>
   /// Register a new customer
   /// </summary>
   /// <param name="requestModel"></param>
   /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequestModel requestModel)
    {
        try
        {
            // check if customer's email is already added
            var customer = await _customerServices.GetCustomerByEmailAsync(requestModel.Email);
            if (customer != null)
            {
                return Conflict("Email is already added");
            }
            
            Authentication.CreatePasswordHash(requestModel.Password,
                out byte[] passwordHash, 
                out byte[] passwordSalt);
          
            var newCustomer = _mapper.Map<Customer>(requestModel);
            
            newCustomer.PasswordHash = passwordHash;
            newCustomer.PasswordSalt = passwordSalt;

            var isSaved = await _customerServices.RegisterCustomerAsync(newCustomer);
            return isSaved
                ? StatusCode((int)HttpStatusCode.Created,"Registered successfully")
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured, try again later");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured registering customer\n{requestModel}", JsonConvert.SerializeObject(requestModel,Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened, try again later");
        }
    }
    
    /// <summary>
    /// Login Customer
    /// </summary>
    /// <param name="requestModel"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginCustomer([FromBody] CustomerLoginRequestModel requestModel)
    {

        try
        {
            var customer = await _customerServices.GetCustomerByEmailAsync(requestModel.Email);
            
            if (!Authentication.VerifyPasswordHash(requestModel.Password, customer.PasswordHash, customer.PasswordSalt))
            {
               return BadRequest("Password is incorrect");
            }

            return Ok("Login successful");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured logging in \n{requestModel}", JsonConvert.SerializeObject(requestModel,Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened, try again later");
        
        }
    }
    
    
    /// <summary>
    /// Get all customers
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers= await _customerServices.GetAllCustomersAsync();
        return Ok(_mapper.Map<IEnumerable<CustomerResponseModel>>(customers));
    }
    
    /// <summary>
    /// Retrieve a product
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpGet("{customerId:required}")]
    public async Task<IActionResult> GetCustomerById([FromRoute]string customerId)
    {
        var customer = await _customerServices.GetCustomerByIdAsync(customerId);
        return Ok(_mapper.Map<CustomerResponseModel>(customer));
    }

    /// <summary>
    /// Update a customer
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{customerId:required}")]
    public async Task<IActionResult> UpdateCustomer([FromRoute] string customerId, [FromBody] RegisterCustomerUpdateModel updateModel)
    {
        try
        {
            var customer =await _customerServices.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }
                
            _mapper.Map(updateModel, customer);
            var isUpdated =  await _customerServices.UpdateCustomerAsync(customer);
            return isUpdated
                ? StatusCode((int)HttpStatusCode.OK, "Customer updated")
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured, try again later");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating Customer\n{updateModel}",JsonConvert.SerializeObject(updateModel,Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened");
        }
    }
    
    /// <summary>
    ///  Delete a customer
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpDelete("{customerId:required}")]
    public async Task<IActionResult> DeleteCustomer([FromRoute] string customerId) 
    {
        try
        {
            var customer = await _customerServices.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }
            
            var isDeleted = await _customerServices.DeleteCustomerAsync(customer);
            return isDeleted
                ? StatusCode((int)HttpStatusCode.OK, "Customer deleted")
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured,try again later");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating Customer\n{customerId}",JsonConvert.SerializeObject(customerId, Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened");
        }
       
    }
    
}