using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTO.Order;
using Models.DTO.Wallet;
using Unit_Of_Work;

namespace E_Commerce_API.Service
{
    public class OrderService
    {
        UnitWork unitWork;
        public OrderService(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        public async Task<List<OrderReadDTO>> GetAllOrder()
        {
            var orders = await unitWork.OrderRepo.GetAllAsync();
            if (orders == null)
                return new List<OrderReadDTO>();

            // جلب كل الـ ProductNames Images مرة واحدة
            var productIds = orders.SelectMany(o => o.Items.Select(i => i.ProductId)).Distinct().ToList();

            var productsDict = await unitWork.db.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => new { p.Name });

            var imagesDict = await unitWork.db.Images
                .Where(im => productIds.Contains(im.ProductId))
                .GroupBy(im => im.ProductId)
                .ToDictionaryAsync(g => g.Key, g => g.First().ImageUrl);

            List<OrderReadDTO> orderDTOs = new List<OrderReadDTO>();
           
                                

            foreach (var order in orders)
            {
                var items = order.Items.Select(i => new GetOrderItemDTO
                {
                    ProductName = productsDict.ContainsKey(i.ProductId) ? productsDict[i.ProductId].Name : "Unknown",
                    ImageUrl = imagesDict.ContainsKey(i.ProductId) ? imagesDict[i.ProductId] : "",
                    Quantity = i.Quantity,
                    ProductId = i.ProductId,
                    UnitPrice = unitWork.db.Products
                                .Where(p => p.Id == i.ProductId)
                                .Select(p => p.Price)
                                .FirstOrDefault()
                }).ToList();
 

                orderDTOs.Add(new OrderReadDTO
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    PaymentMethod = order.PaymentMethod,
                    OrderDate = order.OrderDate,
                    OrderState = order.OrderStatus,
                    OrderItems = items,
                    custInfo = new()
                    {
                        Name = order.CustomerInfo.CustName,
                        Email = order.CustomerInfo.Email,
                        Address = order.CustomerInfo.Address,
                        Phone = order.CustomerInfo.Phone,
                    }
                });
            }

            return orderDTOs;
        }

        public async Task<OrderCreatedDTO> GetOrderById(int id)
        {
            var order = await unitWork.OrderRepo.GetByIdAsync(id);
            if (order == null)
                return null;

           var items = order.Items.Select(i => new OrderItemCreateDTO
           {
               ProductId = i.ProductId,
               Quantity = i.Quantity,
               UnitPrice = unitWork.db.Products
                                .Where(p => p.Id == i.ProductId)
                                .Select(p => p.Price)
                                .FirstOrDefault()

           }).ToList();

            return new OrderCreatedDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                PaymentMethod = order.PaymentMethod,
                OrderDate = order.OrderDate,
                Items = items,
                custInfo = new()
                {
                    Name = order.CustomerInfo.CustName,
                    Email = order.CustomerInfo.Email,
                    Address = order.CustomerInfo.Address,
                    Phone = order.CustomerInfo.Phone
                }
            };
        }

        public Order CreateOrder(OrderCreatedDTO orderDTO)
        {

            var order = new Order()
            {
               
                UserId = orderDTO.UserId,
                PaymentMethod = orderDTO.PaymentMethod,
                OrderDate = DateTime.UtcNow,
                Items = orderDTO.Items.Select(i => new OrderItem()
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = unitWork.db.Products
                                .Where(p => p.Id == i.ProductId)
                                .Select(p => p.Price)
                                .FirstOrDefault()
                }).ToList(),
                CustomerInfo = new CustomerInfo(orderDTO.custInfo),
             

            };
            unitWork.OrderRepo.Add(order);
            unitWork.OrderRepo.Save();
            return order;
        }   
        public void DeleteOrder(int id)
        {
            var order = unitWork.OrderRepo.GetById(id);
            if (order == null)
                throw new InvalidOperationException("Order not found.");
            else
            {
                unitWork.OrderRepo.Delete(id);
                unitWork.Save();
            }
        }
    }

}
