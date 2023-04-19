using AutoFixture;
using AutoMapper;
using GodwinStore.Api.Controllers;
using GodwinStore.Api.Data.StoreData;
using GodwinStore.Api.Model.CustomerModel.RequestModel;
using GodwinStore.Api.Services.CustomerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestStore.UnitTest.TestSetup;
using Xunit;

namespace TestStore.UnitTest;

public class CustomerControllerTests:IClassFixture<TestFixture>
{
    private readonly CustomerController _customerController;
    private readonly Mock<ICustomerServices> _customerServicesMock = new Mock<ICustomerServices>();
    private readonly Fixture _fixture = new Fixture();

    public CustomerControllerTests(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<CustomerController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _customerController = new CustomerController(_customerServicesMock.Object, mapper, logger);
    }
    
    [Fact]
    public async Task Register_Customer_If_AddedAlready_ReturnConflict()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();

        _customerServicesMock.Setup(repos => repos.GetCustomerByEmailAsync(It.IsAny<string>())).ReturnsAsync(customer);

        // Act
        var result = await _customerController.RegisterCustomer(new RegisterCustomerRequestModel()
        {
            Name = customer.Name,
            Contact = customer.Contact,
            Email = customer.Email
        }) as ObjectResult;

        // Assert
        Assert.Equal(409,result?.StatusCode);
    }

    [Fact]
    public async Task Register_Customer_ThrowException()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();
        
        _customerServicesMock.Setup(repos => repos.RegisterCustomerAsync(It.IsAny<Customer>())).ThrowsAsync(new Exception());

        // Act
        var result = await _customerController.RegisterCustomer(new RegisterCustomerRequestModel()
        {
            Name = customer.Name,
            Contact = customer.Contact,
            Email = customer.Email,
        }) as ObjectResult;

        // Assert
        Assert.Equal(500,result?.StatusCode);
    }
    
    [Fact]
    public async Task Register_Customer_ReturnOk()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();
        
        _customerServicesMock.Setup(repos => repos.RegisterCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(true);
        
        // Act
        var result = await _customerController.RegisterCustomer(new RegisterCustomerRequestModel()
        {
            Name = customer.Name,
            Contact = customer.Contact,
            Email = customer.Email
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task Login_Customer_ReturnOk()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();
        
        _customerServicesMock.Setup(repos => repos.GetCustomerByEmailAsync(customer.Email)).ReturnsAsync(customer);
        
        // Act
        var result = await _customerController.LoginCustomer(new CustomerLoginRequestModel()
        {
           Email = customer.Email
        }) as ObjectResult;
  
        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task GetAll_Customers_ReturnsOk()
    {
        // Arrange
        var customers = _fixture.CreateMany<Customer>(3).ToList();

        _customerServicesMock.Setup(repos => repos.GetAllCustomersAsync()).ReturnsAsync(customers);

        // Act
        var result = await _customerController.GetAllCustomers() as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task GetCustomer_ById_ReturnsOk()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();
        
        _customerServicesMock.Setup(repos => repos.GetCustomerByIdAsync(It.IsAny<string>())).ReturnsAsync(customer);
        
        // Act
        var result = await _customerController.GetCustomerById(customer.Id) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }

    [Fact]
    public async Task Update_Customer_IfNull_ReturnNotFound()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();

        _customerServicesMock.Setup(repos => repos.UpdateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(false);

        // Act
        var result = await _customerController.UpdateCustomer(customer.Id,new RegisterCustomerUpdateModel()
        {
            Name = customer.Name,
            Contact = customer.Contact,
            Email = customer.Email,
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Customer_If_NotNull_ReturnOk()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();

        _customerServicesMock.Setup(repos => repos.GetCustomerByIdAsync(customer.Id)).ReturnsAsync(customer);
        _customerServicesMock.Setup(repos => repos.UpdateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(true);

        // Act
        var result = await _customerController.UpdateCustomer( customer.Id,new RegisterCustomerUpdateModel()
        {
            Name = customer.Name,
            Contact = customer.Contact,
            Email = customer.Email,
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Customer_IfNull_ReturnNotFound()
    {
        // Arrange
        _customerServicesMock.Setup(repos => repos.DeleteCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(false);

        // Act
        var result = await _customerController.DeleteCustomer(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Customer_If_NotNull_ReturnOk()
    {
        // Arrange
        var customer = _fixture.Create<Customer>();

        _customerServicesMock.Setup(repos => repos.GetCustomerByIdAsync(customer.Id))
            .ReturnsAsync(customer);
            
        _customerServicesMock.Setup(repos => repos.DeleteCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(true);

        // Act
        var result = await _customerController.DeleteCustomer(customer.Id) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
}