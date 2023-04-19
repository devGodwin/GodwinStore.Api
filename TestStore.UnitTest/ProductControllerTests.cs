using AutoFixture;
using AutoMapper;
using GodwinStore.Api.Controllers;
using GodwinStore.Api.Data;
using GodwinStore.Api.Model.ProductModel.RequestModel;
using GodwinStore.Api.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestStore.UnitTest.TestSetup;
using Xunit;

namespace TestStore.UnitTest;

public class ProductControllerTests:IClassFixture<TestFixture>
{
    private readonly ProductController _productController;
    private readonly Mock<IProductServices> _productServicesMock = new Mock<IProductServices>();
    private readonly Fixture _fixture = new Fixture();

    public ProductControllerTests(TestFixture fixture)
    {
        var logger = fixture.ServiceProvider.GetService<ILogger<ProductController>>();
        var mapper = fixture.ServiceProvider.GetService<IMapper>();

        _productController = new ProductController(_productServicesMock.Object, mapper, logger);
    }
    
    [Fact]
    public async Task Create_Product_If_AddedAlready_ReturnConflict()
    {
        // Arrange
        var product = _fixture.Create<Product>();

        _productServicesMock.Setup(repos => repos.GetProductByProductNameAsync(product.ProductName)).ReturnsAsync(product);

        // Act
        var result = await _productController.CreateProduct(new ProductRequestModel()
        {
            ProductName = product.ProductName,
            Description = product.Description,
            ProductPrice = product.ProductPrice,
            ImageUrl = product.ImageUrl
        }) as ObjectResult;

        // Assert
        Assert.Equal(409,result?.StatusCode);
    }
    
    [Fact]
    public async Task Create_Product_ReturnOk()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        
        _productServicesMock.Setup(repos => repos.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(true);

        // Act
        var result = await _productController.CreateProduct(new ProductRequestModel()
        {
            ProductName = product.ProductName,
            Description = product.Description,
            ProductPrice = product.ProductPrice,
            ImageUrl = product.ImageUrl
        }) as ObjectResult;

        // Assert
        Assert.Equal(201,result?.StatusCode);
    }
    
    [Fact]
    public async Task Create_Product_ThrowException()
    {
        var product = _fixture.Create<Product>();
        
        _productServicesMock.Setup(repos => repos.CreateProductAsync(It.IsAny<Product>())).ThrowsAsync(new Exception());

        var result = await _productController.CreateProduct(new ProductRequestModel()
        {
            ProductName = product.ProductName,
            Description = product.Description,
            ProductPrice = product.ProductPrice,
            ImageUrl = product.ImageUrl
        }) as ObjectResult;

        Assert.Equal(500,result?.StatusCode);
    }
    
    [Fact]
    public async Task GetAll_Products_ReturnsOk()
    {
        // Arrange
        var products = _fixture.CreateMany<Product>(3).ToList();

        _productServicesMock.Setup(repos => repos.GetAllProductsAsync()).ReturnsAsync(products);

        // Act
        var result = await _productController.GetAllProducts() as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task GetProduct_ById_ReturnsOk()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        
        _productServicesMock.Setup(repos => repos.GetProductByIdAsync(It.IsAny<string>())).ReturnsAsync(product);
        
        // Act
        var result = await _productController.GetProductById(product.Id) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }

    [Fact]
    public async Task Update_Product_IfNull_ReturnNotFound()
    {
        // Arrange
        var product = _fixture.Create<Product>();

        _productServicesMock.Setup(repos => repos.UpdateProductAsync(It.IsAny<Product>())).ReturnsAsync(false);

        // Act
        var result = await _productController.UpdateProduct(product.Id,new ProductUpdateModel()
        {
            ProductName = product.ProductName,
            Description = product.Description,
            ProductPrice = product.ProductPrice,
            ImageUrl = product.ImageUrl
        }) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Update_Product_If_NotNull_ReturnOk()
    {
        // Arrange
        var product = _fixture.Create<Product>();

        _productServicesMock.Setup(repos => repos.GetProductByIdAsync(product.Id)).ReturnsAsync(product);
        _productServicesMock.Setup(repos => repos.UpdateProductAsync(It.IsAny<Product>())).ReturnsAsync(true);

        // Act
        var result = await _productController.UpdateProduct( product.Id,new ProductUpdateModel()
        {
            ProductName = product.ProductName,
            Description = product.Description,
            ProductPrice = product.ProductPrice,
            ImageUrl = product.ImageUrl
        }) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Product_IfNull_ReturnNotFound()
    {
        // Arrange
        _productServicesMock.Setup(repos => repos.DeleteProductAsync(It.IsAny<Product>())).ReturnsAsync(false);

        // Act
        var result = await _productController.DeleteProduct(It.IsAny<string>()) as ObjectResult;

        // Assert
        Assert.Equal(404,result?.StatusCode);
    }
    
    [Fact]
    public async Task Delete_Product_If_NotNull_ReturnOk()
    {
        // Arrange
        var product = _fixture.Create<Product>();

        _productServicesMock.Setup(repos => repos.GetProductByIdAsync(product.Id))
            .ReturnsAsync(product);
            
        _productServicesMock.Setup(repos => repos.DeleteProductAsync(It.IsAny<Product>())).ReturnsAsync(true);

        // Act
        var result = await _productController.DeleteProduct(product.Id) as ObjectResult;

        // Assert
        Assert.Equal(200,result?.StatusCode);
    }
    
}