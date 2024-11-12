using CRUD.DBContext;
using CRUD.DTO;
using CRUD.Helper;
using CRUD.IRepository;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRUD.Repository
{
    public class Order : IOrder
    {
        private readonly AppDbContext _context;
        public Order(AppDbContext context)
        {
            _context = context;
        }

        // API 00: Insert Product
        public async Task<MessageHelper> InsertProduct(ProductDTO product)
        {
            try
            {
                var newProduct = new TblProduct
                {
                    StrProductName = product.ProductName,
                    NumUnitPrice = product.UnitPrice,
                    NumStock = product.Stock
                };

                await _context.TblProducts.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                return new MessageHelper
                {
                    Message = "Product added successfully.",
                    StatusCode = 201
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        // API 01: Create Order
        public async Task<OrderMessageHelper> CreateOrder(OrderDTO orderDTO)
        {
            try
            {
                var productNameAfterTrimming = orderDTO.ProductName.Trim();
                var customerNameAfterTrimming = orderDTO.CustomerName.Trim();

                // Find the product name
                var product = await _context.TblProducts.FirstOrDefaultAsync(p => p.StrProductName == productNameAfterTrimming);

                if (product == null)
                {
                    throw new Exception("Product not found.");
                }

                if (product.NumStock < orderDTO.Quantity)
                {
                    throw new Exception("Insufficient stock available.");
                }

                //If Qty <= 0
                if (orderDTO.Quantity <= 0)
                {
                    throw new Exception("Quantity must be greater than 0.");
                }

                // Create order
                var newOrder = new TblOrder
                {
                    IntProductId = product.IntProductId,  
                    StrCustomerName = customerNameAfterTrimming,
                    NumQuantity = orderDTO.Quantity,
                    DteOrderDateTime = DateTime.Now,
                    IsActive = true,
                    DteLastActionDateTime = DateTime.Now
                };

                await _context.TblOrders.AddAsync(newOrder);

                // Deduct ordered quantity
                product.NumStock -= orderDTO.Quantity;

                await _context.SaveChangesAsync();

                return new OrderMessageHelper
                {
                    Message = "Order placed successfully.",
                    StatusCode = 201,
                    NewOrderId = newOrder.IntOrderId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        // API 02: Update Order Qty
        public async Task<MessageHelper> UpdateOrderQuantity(int orderId, int newQuantity)
        {
            try
            {
                // Find the order
                var order = await _context.TblOrders.FirstOrDefaultAsync(o => o.IntOrderId == orderId);
                if (order == null)
                {
                    throw new Exception("Order not found.");
                }

                var product = await _context.TblProducts.FirstOrDefaultAsync(p => p.IntProductId == order.IntProductId);
                if (product == null)
                {
                    throw new Exception("That Product not found.");
                }

                // if stock available
                if (product.NumStock + order.NumQuantity < newQuantity)
                {
                    throw new Exception("Insufficient stock available.");
                }

                var quantityDifference = newQuantity - order.NumQuantity;
                order.NumQuantity = newQuantity;        // Update Order Qty
                product.NumStock -= quantityDifference; // Update Stock Qty
                order.DteLastActionDateTime = DateTime.Now; //LastAction Update

                await _context.SaveChangesAsync();

                return new MessageHelper
                {
                    Message = "Order quantity updated successfully.",
                    StatusCode = 200
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        //API 03: Delete Order
        public async Task<MessageHelper> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _context.TblOrders.FirstOrDefaultAsync(o => o.IntOrderId == orderId);
                if (order == null)
                {
                    throw new Exception("Order not found.");
                }

                var product = await _context.TblProducts.FirstOrDefaultAsync(p => p.IntProductId == order.IntProductId);
                if (product == null)
                {
                    throw new Exception("Product not found.");
                }

                product.NumStock += order.NumQuantity; // Add qty to stock
                order.IsActive = false;                // Soft delete
                order.DteLastActionDateTime = DateTime.Now;

                await _context.SaveChangesAsync();

                return new MessageHelper
                {
                    Message = "Order is deleted and stock updated successfully.",
                    StatusCode = 200
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

       // API 06: Get products below the quantity
        public async Task<List<ProductDTO>> GetProductsBelowQuantity(decimal quantity) 
        {
            try
            {
                var products = await _context.TblProducts
                    .Where(p => p.NumStock < quantity)
                    .Select(p => new ProductDTO
                    {
                        ProductName = p.StrProductName,
                        UnitPrice = p.NumUnitPrice,
                        Stock = p.NumStock
                    })
                    .ToListAsync();
                if(!products.Any())
                {
                    throw new Exception("No products found below the specified quantity.");
                }
                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
