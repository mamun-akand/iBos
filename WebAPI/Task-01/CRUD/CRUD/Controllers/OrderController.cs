using CRUD.DTO;
using CRUD.Helper;
using CRUD.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _IOrderRepo; 
        public OrderController(IOrder IOrderRepo)
        {
            _IOrderRepo = IOrderRepo;
        }

        // API 00: Insert
        [HttpPost]
        [Route("InsertProduct")]
        public async Task<MessageHelper> InsertProduct(ProductDTO product)
        {
            var result = await _IOrderRepo.InsertProduct(product);
            return result; 
        }

        //API 01
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<OrderMessageHelper> CreateOrder(OrderDTO orderDTO)
        {
            var result = await _IOrderRepo.CreateOrder(orderDTO);
            return result;
        }

        // API 02: Update quantity
        [HttpPut]
        [Route("UpdateOrderQuantity")]
        public async Task<MessageHelper> UpdateOrderQuantity(int orderId, int newQuantity)
        {
            var result = await _IOrderRepo.UpdateOrderQuantity(orderId, newQuantity);
            return result;
        }

        //API 03: Delete Order
        [HttpDelete]
        [Route("DeleteOrder")]
        public async Task<MessageHelper> DeleteOrder(int orderId)
        {
            var result = await _IOrderRepo.DeleteOrder(orderId);
            return result;
        }


        // API 06: Get products below the quantity
        [HttpGet]
        [Route("GetProductsBelowQuantity")]
        public async Task<IActionResult> GetProductsBelowQuantity(decimal quantity)
        {
            var products = await _IOrderRepo.GetProductsBelowQuantity(quantity);

            return Ok(products);
        }
    }
}
