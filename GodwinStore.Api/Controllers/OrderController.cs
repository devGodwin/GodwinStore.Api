using System.Net;
using AutoMapper;
using GodwinStore.Api.Data;
using GodwinStore.Api.Data.StoreData;
using GodwinStore.Api.Model.CustomerModel.RequestModel;
using GodwinStore.Api.Model.CustomerModel.ResponseModel;
using GodwinStore.Api.Model.OrderModel.RequestModel;
using GodwinStore.Api.Model.OrderModel.ResponseModel;
using GodwinStore.Api.Services.CustomerServices;
using GodwinStore.Api.Services.OrderServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GodwinStore.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController:ControllerBase
{
    private readonly IOrderServices _orderServices;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderServices orderServices,IMapper mapper, ILogger<OrderController> logger)
    {
        _orderServices = orderServices;
        _mapper = mapper;
        _logger = logger;
    }
    
    
   /// <summary>
   /// Place a new order
   /// </summary>
   /// <param name="requestModel"></param>
   /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestModel requestModel)
    {
        try
        {
          /*  
            // check if order is already placed
            var order = await _orderServices.GetOrderByShippingAddressAsync(new CustomerRequestModel().ShippingAddress);
            if (order != null)
            {
                return Conflict("Order is already placed");
            }
         */   
            var newOrder = _mapper.Map<Order>(requestModel);
            newOrder.TotalPrice = newOrder.Quantity * newOrder.UnitPrice;
            newOrder.Status="Items ordered successfully";

            var isSaved = await _orderServices.PlaceOrderAsync(newOrder);
            return isSaved
                ? StatusCode((int)HttpStatusCode.Created,newOrder)
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured, try again later");

        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured placing Order\n{requestModel}", JsonConvert.SerializeObject(requestModel,Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened, try again later");
        }
    }
    
    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders= await _orderServices.GetAllOrdersAsync();
        return Ok(_mapper.Map<IEnumerable<OrderResponseModel>>(orders));
    }
    
    /// <summary>
    /// Retrieve order
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpGet("{orderId:required}")]
    public async Task<IActionResult> GetOrderById([FromRoute]string orderId)
    {
        var order = await _orderServices.GetOrderByIdAsync(orderId);
        return Ok(_mapper.Map<OrderResponseModel>(order));
    }

    /// <summary>
    /// Update order
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    [HttpPut("{orderId:required}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] string orderId, [FromBody] OrderUpdateModel updateModel)
    {
        try
        {
            var order =await _orderServices.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found");
            }
                
            _mapper.Map(updateModel, order);
            order.TotalPrice = order.Quantity * order.UnitPrice;
            
            var isUpdated =  await _orderServices.UpdateOrderAsync(order);
            return isUpdated
                ? StatusCode((int)HttpStatusCode.OK, "Order updated")
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured, try again later");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating Order\n{updateModel}",JsonConvert.SerializeObject(updateModel,Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened");
        }
    }
    
    /// <summary>
    ///  Delete order
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpDelete("{orderId:required}")]
    public async Task<IActionResult> DeleteOrder([FromRoute] string orderId) 
    {
        try
        {
            var order = await _orderServices.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            
            var isDeleted = await _orderServices.DeleteOrderAsync(order);
            return isDeleted
                ? StatusCode((int)HttpStatusCode.OK, "Order deleted")
                : StatusCode((int)HttpStatusCode.FailedDependency, "An error occured,try again later");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An error occured updating Order\n{customerId}",JsonConvert.SerializeObject(orderId, Formatting.Indented));
            return StatusCode((int)HttpStatusCode.InternalServerError, "Something bad happened");
        }
       
    }
    
}