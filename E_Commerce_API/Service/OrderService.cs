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
        public async Task<List<OrderDTO>> GetAllOrder()
        {
            var orders = await unitWork.OrderRepo.GetAllAsync();
            if (orders == null)
                return new List<OrderDTO>(); // Return an empty list 
            else
            {
                List<OrderDTO> orderDTOs = new List<OrderDTO>();
                foreach (Order order in orders)
                {
                    OrderDTO orderDTO = new OrderDTO()
                    {
                        Id = order.Id,
                        RegisterId = order.RegisterId,
                        RegisterName = unitWork.RegisterRepo.GetById(order.RegisterId).FullName, 
                        RegisterEmail = unitWork.RegisterRepo.GetById(order.RegisterId).Email,
                        Address = order.Address,
                        RegisterPhoneNumber = unitWork.RegisterRepo.GetById(order.RegisterId).PhoneNumber,
                        TotalPrice = order.TotalPrice,
                        PaymentMethod = order.PaymentMethod,
                        OrderDate = order.OrderDate.ToString(),
                        Status = order.Status
                    };
                    orderDTOs.Add(orderDTO);
                }
                return orderDTOs; 
            }
        }
        public async Task<OrderDTO> GetOrderById(int id)
        {
            var order = await unitWork.OrderRepo.GetByIdAsync(id);
            if (order == null)
                return null; // Return null if not found
            else
            {
                OrderDTO orderDTO = new OrderDTO()
                {
                    Id = order.Id,
                    RegisterId = order.RegisterId,
                    RegisterName = unitWork.RegisterRepo.GetById(order.RegisterId).FullName,
                    RegisterEmail = unitWork.RegisterRepo.GetById(order.RegisterId).Email,
                    Address = order.Address,
                    RegisterPhoneNumber = unitWork.RegisterRepo.GetById(order.RegisterId).PhoneNumber,
                    TotalPrice = order.TotalPrice,
                    PaymentMethod = order.PaymentMethod,
                    OrderDate = order.OrderDate.ToString(),
                    Status = order.Status
                };
                return orderDTO;
            }
        }
        public void PostOrder(OrderDTO orderDTO)
        {
            if (orderDTO.PaymentMethod.Equals("BankTransfer"))
            {
                var buyerWallet = unitWork.db.Wallets.FirstOrDefault(w => w.UserId == orderDTO.RegisterId);
                if (buyerWallet == null)
                    throw new InvalidOperationException("Wallet not found for the user.");


                if (buyerWallet.Balance < orderDTO.TotalPrice)
                    throw new InvalidOperationException("Insufficient balance in the wallet.");


                buyerWallet.Balance -= (decimal)orderDTO.TotalPrice;
                var now = DateTime.Now;
                buyerWallet.Transactions.Add(new WalletTransaction()
                {
                    Amount = (decimal)orderDTO.TotalPrice,
                    TransactionType = TransactionType.Withdrawal.ToString(),
                    TransactionDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0),
                    Description = "Order payment",
                    WalletId = buyerWallet.Id,
                    WalletName = unitWork.db.Wallets.FirstOrDefault(w => w.UserId == orderDTO.RegisterId).UserName
                });

                var systemWallet = unitWork.db.Wallets.FirstOrDefault(w => w.UserName == "Infinity");
                if (systemWallet == null)
                    throw new InvalidOperationException("System wallet not found.");
               
                systemWallet.Balance += (decimal)orderDTO.TotalPrice;
                systemWallet.Transactions.Add(new WalletTransaction()
                {
                    Amount = (decimal)orderDTO.TotalPrice,
                    TransactionType = TransactionType.Deposit.ToString(),
                    TransactionDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0),
                    Description = $"Received payment from user {orderDTO.RegisterName}",
                    WalletId = systemWallet.Id,
                    WalletName = unitWork.db.Wallets.Where(w => w.UserName == "Infinity").FirstOrDefault().UserName
                });

                unitWork.db.Wallets.Update(buyerWallet);
                unitWork.db.Wallets.Update(systemWallet);

                Order order = new Order()
                {
                    RegisterId = orderDTO.RegisterId,
                    Address = orderDTO.Address,
                    TotalPrice = (decimal)orderDTO.TotalPrice,
                    PaymentMethod = orderDTO.PaymentMethod,
                    OrderDate = orderDTO.OrderDate,
                    Status = orderDTO.Status,
                    RegisterName = orderDTO.RegisterName,
                    RegisterEmail = orderDTO.RegisterEmail,
                    PhoneNumber = orderDTO.RegisterPhoneNumber,
                };
                unitWork.OrderRepo.Add(order);
                unitWork.Save();
            }
            else
            {
                Order order = new Order()
                {
                    RegisterId = orderDTO.RegisterId,
                    Address = orderDTO.Address,
                    TotalPrice = (decimal)orderDTO.TotalPrice,
                    PaymentMethod = orderDTO.PaymentMethod,
                    OrderDate = orderDTO.OrderDate,
                    Status = orderDTO.Status,
                    RegisterName = orderDTO.RegisterName,
                    RegisterEmail = orderDTO.RegisterEmail,
                    PhoneNumber = orderDTO.RegisterPhoneNumber,
                };
                unitWork.OrderRepo.Add(order);
                unitWork.Save();
            }
        }
        public void UpdateOrder(int id, OrderDTO orderDTO)
        {
            var order = unitWork.OrderRepo.GetById(id);
            if (order == null)
                throw new InvalidOperationException("Order not found.");
            else
            {
                order.RegisterId = orderDTO.RegisterId;
                order.Address = orderDTO.Address;
                order.TotalPrice = (decimal)orderDTO.TotalPrice;
                order.PaymentMethod = orderDTO.PaymentMethod;
                order.OrderDate = orderDTO.OrderDate;
                order.Status = orderDTO.Status;
                unitWork.OrderRepo.Update(order,id);
                unitWork.Save();
            }
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
