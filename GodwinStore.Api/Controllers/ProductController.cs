using System.Net;
using AutoMapper;
using GodwinStore.Api.Data;
using GodwinStore.Api.Model.ProductModel.RequestModel;
using GodwinStore.Api.Model.ProductModel.ResponseModel;
using GodwinStore.Api.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GodwinStore.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController:ControllerBase
{
    private readonly IProductServices _productServices;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductServices productServices,IMapper mapper, ILogger<ProductController> logger)
    {
        _productServices = productServices;
        _mapper = mapper;
        _logger = logger;
    }
    
    
   /// <summary>
   /// Create a new product
   /// </summary>
   /// <param name="requestModel"></param>
   /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequestModel requestModel)
    {
        try
        {
            // check if product is already added
            var product = await _productServices.GetProductByProductNameAsync(requestModel.ProductName);
            if (product != null)
            {
                return Conflict("Product is already added");
            }
            
            var newProduct = _mapper.Map<Product>(requestModel);

            var isSaved = await _productServices.CreateProductAsync(newProduct);
            return isSaved
                ? StatusCode((int)HttpStatusCode.Created,newProduct)
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured, try again later");

        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured creating product\n{requestModel}", JsonConvert.SerializeObject(requestModel,Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened, try again later");
        }
    }
    
    /// <summary>
    /// Get all product
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products= await _productServices.GetAllProductsAsync();
        return Ok(_mapper.Map<IEnumerable<ProductResponseModel>>(products));
    }
    
    /// <summary>
    /// Retrieve a product
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{productId:required}")]
    public async Task<IActionResult> GetProductById([FromRoute]string productId)
    {
        var product = await _productServices.GetProductByIdAsync(productId);
        return Ok(_mapper.Map<ProductResponseModel>(product));
    }

    /// <summary>
    /// Update a product
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{productId:required}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] string productId, [FromBody] ProductUpdateModel updateModel)
    {
        try
        {
            var product =await _productServices.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }
                
            _mapper.Map(updateModel, product);
            var isUpdated =  await _productServices.UpdateProductAsync(product);
            return isUpdated
                ? StatusCode((int)HttpStatusCode.OK, "Product updated")
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured, try again later");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating product\n{updateModel}",JsonConvert.SerializeObject(updateModel,Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened");
        }
    }
    
    /// <summary>
    ///  Delete a product
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpDelete("{productId:required}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] string productId) 
    {
        try
        {
            var product = await _productServices.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            
            var isDeleted = await _productServices.DeleteProductAsync(product);
            return isDeleted
                ? StatusCode((int)HttpStatusCode.OK, "Product deleted")
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured,try again later");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating product\n{productId}",JsonConvert.SerializeObject(productId, Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened");
        }
       
    }
    
}