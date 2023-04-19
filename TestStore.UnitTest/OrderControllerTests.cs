using AutoFixture;
using AutoMapper;
using GodwinStore.Api.Controllers;
using GodwinStore.Api.Data;
using GodwinStore.Api.Data.StoreData;
using GodwinStore.Api.Model.CustomerModel.RequestModel;
using GodwinStore.Api.Model.OrderModel.RequestModel;
using GodwinStore.Api.Services.CustomerServices;
using GodwinStore.Api.Services.OrderServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestStore.UnitTest.TestSetup;
using Xunit;

namespace TestStore.UnitTest;

public class OrderControllerTests:IClassFixture<TestFixture>
{
    private readonly OrderController _orderController;
    private readonly Mock<IOrderServices> _orderServicesMock = new Mock<IOrderServices>();
    private readonly Fixture _fixture = new Fixture();

    public OrderControllerTests(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<OrderController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _orderController = new OrderController(_orderServicesMock.Object, mapper, logger);
    }
    
   /* [Fact]
    public async Task Place_Order_If_AddedAlready_ReturnConflict()
    {
        // Arrange
        var order = _fixture.Create<Order>();

        _orderServicesMock.Setup(repos => repos.GetOrderByShippingAddressAsync(It.IsAny<string>())).ReturnsAsync(order);

        // Act
        var result = await _orderController.PlaceOrder(new OrderRequestModel()
        {
            UnitPrice = order.UnitPrice,
            Quantity = order.Quantity
        }) as ObjectResult;

        // Assert
        Assert.Equal(409,result?.StatusCode);
    }*/
    
    [Fact]
    public async Task Place_Order_ReturnOk()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        _orderServicesMock.Setup(repos => repos.PlaceOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

        // Act
        var result = await _orderController.PlaceOrder(new OrderRequestModel()
        {
            Quantity = order.Quantity,
            UnitPrice = order.UnitPrice
        }) as ObjectResult;

        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task Place_Order_ThrowException()
    {
        var order = _fixture.Create<Order>();
        
        _orderServicesMock.Setup(repos => repos.PlaceOrderAsync(It.IsAny<Order>())).ThrowsAsync(new Exception());

        var result = await _orderController.PlaceOrder(new OrderRequestModel()
        {
            Quantity = order.Quantity,
           UnitPrice = order.UnitPrice
        }) as ObjectResult;

        Assert.Equal(500,result?.StatusCode);
    }
    
    [Fact]
    public async Task GetAll_Orders_ReturnsOk()
    {
        // Arrange
        var orders = _fixture.CreateMany<Order>(3).ToList();

        _orderServicesMock.Setup(repos => repos.GetAllOrdersAsync()).ReturnsAsync(orders);

        // Act
        var result = await _orderController.GetAllOrders() as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task GetOrder_ById_ReturnsOk()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        
        _orderServicesMock.Setup(repos => repos.GetOrderByIdAsync(It.IsAny<string>())).ReturnsAsync(order);
        
        // Act
        var result = await _orderController.GetOrderById(order.Id) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }

    [Fact]
    public async Task Update_Order_IfNull_ReturnNotFound()
    {
        // Arrange
        var order = _fixture.Create<Order>();

        _orderServicesMock.Setup(repos => repos.UpdateOrderAsync(It.IsAny<Order>())).ReturnsAsync(false);

        // Act
        var result = await _orderController.UpdateOrder(order.Id,new OrderUpdateModel()
        {
           CustomerName = order.CustomerName,
           Quantity = order.Quantity,
           ShippingAddress = order.ShippingAddress,
           UnitPrice = order.UnitPrice
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Order_If_NotNull_ReturnOk()
    {
        // Arrange
        var order = _fixture.Create<Order>();

        _orderServicesMock.Setup(repos => repos.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);
        _orderServicesMock.Setup(repos => repos.UpdateOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

        // Act
        var result = await _orderController.UpdateOrder( order.Id,new OrderUpdateModel()
        {
            CustomerName = order.CustomerName,
            Quantity = order.Quantity,
            ShippingAddress = order.ShippingAddress,
            UnitPrice = order.UnitPrice
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Order_IfNull_ReturnNotFound()
    {
        // Arrange
        _orderServicesMock.Setup(repos => repos.DeleteOrderAsync(It.IsAny<Order>())).ReturnsAsync(false);

        // Act
        var result = await _orderController.DeleteOrder(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Order_If_NotNull_ReturnOk()
    {
        // Arrange
        var order = _fixture.Create<Order>();

        _orderServicesMock.Setup(repos => repos.GetOrderByIdAsync(order.Id))
            .ReturnsAsync(order);
            
        _orderServicesMock.Setup(repos => repos.DeleteOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

        // Act
        var result = await _orderController.DeleteOrder(order.Id) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
}