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
                if (product.NumStock < newQuantity)
                {
                    throw new Exception("Insufficient stock available.");
                }

                var quantityDifference = newQuantity - order.NumQuantity;
                order.NumQuantity = newQuantity;        // Update Order Qty
                product.NumStock -= quantityDifference; // Update Stock Qty
                order.DteLastActionDateTime = DateTime.Now; //LastAction Update
                _context.TblOrders.Update(order);
                _context.TblProducts.Update(product);
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

        // API 04: Get all active orders
        public async Task<List<OrderDetailsDTO>> GetAllOrdersWithProductDetails()
        {
            try
            {
                var orders = await (from order in _context.TblOrders
                                    join product in _context.TblProducts on order.IntProductId equals product.IntProductId
                                    where order.IsActive
                                    select new OrderDetailsDTO
                                    {
                                        OrderID = order.IntOrderId,
                                        OrderDate = order.DteOrderDateTime,
                                        Quantity = order.NumQuantity,
                                        ProductName = product.StrProductName,
                                        UnitPrice = product.NumUnitPrice,
                                        CustomerName = order.StrCustomerName
                                    }).ToListAsync();
                if (!orders.Any())
                {
                    throw new Exception("No Order Found.");
                }
                return orders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // API 05: Get product summary
        public async Task<List<ProductSummaryDTO>> GetProductSummary()
        {
            try
            {
                var ordersWithProducts = await (from order in _context.TblOrders

                                                join product in _context.TblProducts on order.IntProductId equals product.IntProductId
                                                group new { order, product } by new { product.StrProductName, product.NumUnitPrice } into gr
                                                select new ProductSummaryDTO
                                                {
                                                    ProductName = gr.Key.StrProductName,
                                                    TotalQuantityOrdered = gr.Sum(x => x.order.NumQuantity),
                                                    TotalRevenue = gr.Sum(x => x.order.NumQuantity) * gr.Key.NumUnitPrice
                                                }).ToListAsync();

                //var productSummary = ordersWithProducts
                //    .GroupBy(o => o.StrProductName)
                //    .Select(g => new ProductSummaryDTO
                //    {
                //        ProductName = g.FirstOrDefault()?.StrProductName,
                //        TotalQuantityOrdered = g.Sum(o => o.NumQuantity),
                //        TotalRevenue = g.Sum(o => o.NumQuantity * o.NumUnitPrice)
                //    })
                //    .ToList();

                return ordersWithProducts;
            }
            catch (Exception ex)
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
                if (!products.Any())
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

        //API 07: Top 3 Customers by Qty
        public async Task<List<Top3CustomersDTO>> GetTop3CustomersByQuantity()
        {
            try
            {
                var topCustomers = await _context.TblOrders
                                                .Where(order => order.IsActive)
                                                .GroupBy(order => order.StrCustomerName)
                                                .OrderByDescending(group => group.Sum(order => order.NumQuantity))
                                                .Take(3)
                                                .Select(group => new Top3CustomersDTO
                                                {
                                                    CustomerName = group.FirstOrDefault().StrCustomerName,
                                                    TotalQuantityOrdered = group.Sum(order => order.NumQuantity)
                                                })
                                                .ToListAsync();
                if (!topCustomers.Any())
                {
                    throw new Exception("No Customer is found");
                }

                return topCustomers;
            }
            catch (Exception)
            {
                throw;
            }


        }

        //API 08: Get Unordered Products
        public async Task<List<string>> GetUnorderedProducts()
        {
            try
            {
                var unorderedProducts = await _context.TblProducts.Where(product => !_context.TblOrders
                                                                                    .Any(order => order.IntProductId == product.IntProductId && order.IsActive == true))
                                                                  .Select(product => product.StrProductName)
                                                                  .ToListAsync();
                if (!unorderedProducts.Any())
                {
                    throw new Exception("No Unordered Product found");
                }

                return unorderedProducts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //API 09: Bulk Order with Transaction
        public async Task<MessageHelper> CreateBulkOrders(List<OrderDTO> orders)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newList = new List<TblOrder>(orders.Count());
                foreach (var order in orders)
                {
                    var product = await _context.TblProducts
                        .FirstOrDefaultAsync(p => p.StrProductName == order.ProductName);

                    if (product == null)
                    {
                        throw new Exception(order.ProductName + " Not Found. So, Rolled Back");
                    }

                    if (product.NumStock < order.Quantity)
                    {
                        throw new Exception(order.ProductName + " is Insufficient Stock. So, Rolled Back");
                    }

                    // Reduce stock and create order
                    product.NumStock -= order.Quantity;

                    var objProduct = new TblOrder
                    {
                        StrCustomerName = order.CustomerName,
                        IntProductId = product.IntProductId,
                        NumQuantity = order.Quantity,
                        DteOrderDateTime = DateTime.Now,
                        IsActive = true,
                        DteLastActionDateTime = DateTime.Now
                    };
                    newList.Add(objProduct);
                    
                }

                await _context.TblOrders.AddRangeAsync(newList);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new MessageHelper
                {
                    Message = "Bulk orders created successfully.",
                    StatusCode = 200
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



    }

}
