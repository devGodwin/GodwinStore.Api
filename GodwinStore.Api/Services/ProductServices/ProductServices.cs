using GodwinStore.Api.Data;
using GodwinStore.Api.Data.DbContexts;
using GodwinStore.Api.Model.ProductModel.RequestModel;
using GodwinStore.Api.Model.ProductModel.ResponseModel;
using GodwinStore.Api.Redis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GodwinStore.Api.Services.ProductServices;

public class ProductServices:IProductServices
{
    private readonly ProductContext _productContext;

    public ProductServices(ProductContext productContext)
    {
        _productContext = productContext;
    }

    public async Task<bool> CreateProductAsync(Product product)
    { 
        await _productContext.Product.AddAsync(product);
     var rows= await _productContext.SaveChangesAsync();
     return rows > 0;
    }

    public async Task<Product> GetProductByProductNameAsync(string productName)
    {
        return await _productContext.Product.FirstOrDefaultAsync(x => x.ProductName.Equals(productName));
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productContext.Product.AsNoTracking().ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
      return await _productContext.Product.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
       _productContext.Product.Update(product);
      var rows= await _productContext.SaveChangesAsync();
      return rows > 0;
    }

    public async Task<bool> DeleteProductAsync(Product product)
    {
        _productContext.Product.Remove(product);
       var rows = await _productContext.SaveChangesAsync();
        return rows > 0;
    }
}