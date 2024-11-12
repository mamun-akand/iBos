using CRUD.DTO;
using CRUD.Helper;

namespace CRUD.IRepository
{
    public interface IOrder
    {
        Task<MessageHelper> InsertProduct(ProductDTO product); // API 00
        Task<OrderMessageHelper> CreateOrder(OrderDTO orderDTO); // API 01 
        Task<MessageHelper> UpdateOrderQuantity(int orderId, int newQuantity); //API 02
        Task<MessageHelper> DeleteOrder(int orderId);  // API 03
        Task<List<ProductDTO>> GetProductsBelowQuantity(decimal quantity);  // API 06
    }
}
