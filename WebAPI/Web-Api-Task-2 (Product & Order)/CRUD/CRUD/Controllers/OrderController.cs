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
        public async Task<IActionResult> InsertProduct(ProductDTO product)
        {
            var result = await _IOrderRepo.InsertProduct(product);
            return Ok(result); 
        }

        //API 01: 
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderDTO orderDTO)
        {
            var result = await _IOrderRepo.CreateOrder(orderDTO);
            return Ok(result);
        }

        // API 02: Update quantity
        [HttpPut]
        [Route("UpdateOrderQuantity")]
        public async Task<IActionResult> UpdateOrderQuantity(int orderId, int newQuantity)
        {
            var result = await _IOrderRepo.UpdateOrderQuantity(orderId, newQuantity);
            return Ok(result);
        }

        //API 03: Delete Order
        [HttpDelete]
        [Route("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await _IOrderRepo.DeleteOrder(orderId);
            return Ok(result);
        }

        // API 04: Get active orders
        [HttpGet]
        [Route("GetOrdersWithProductDetails")]
        public async Task<IActionResult> GetOrdersWithProductDetails()
        {
            var orders = await _IOrderRepo.GetAllOrdersWithProductDetails();
            return Ok(orders);
        }

        // API 05: Get product summary
        [HttpGet]
        [Route("GetProductSummary")]
        public async Task<IActionResult> GetProductSummary()
        {
            var productSummary = await _IOrderRepo.GetProductSummary();
            return Ok(productSummary);
        }


        // API 06: Get products below quantity
        [HttpGet]
        [Route("GetProductsBelowQuantity")]
        public async Task<IActionResult> GetProductsBelowQuantity(decimal quantity)
        {
            var products = await _IOrderRepo.GetProductsBelowQuantity(quantity);

            return Ok(products);
        }

        // API 07: top 3 customers
        [HttpGet]
        [Route("GetTop3Customers")]
        public async Task<IActionResult> GetTop3Customers()
        {
            var topCustomers = await _IOrderRepo.GetTop3CustomersByQuantity();
            return Ok(topCustomers);
        }

        //API 08: Unordered Product List
        [HttpGet]
        [Route("GetUnorderedProducts")]
        public async Task<IActionResult> GetUnorderedProducts()
        {
            var Unordered = await _IOrderRepo.GetUnorderedProducts();
            return Ok(Unordered);
        }

        //API 09: Bulk Order
        [HttpPost]
        [Route("CreateBulkOrders")]
        public async Task<IActionResult> CreateBulkOrders(List<OrderDTO> orders)
        {
            var result = await _IOrderRepo.CreateBulkOrders(orders);
            return StatusCode(result.StatusCode, result.Message);
        }


    }
}
