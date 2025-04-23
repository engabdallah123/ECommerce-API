using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTO.Order;
using Models.DTO.Wallet;
using System.Threading.Tasks;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        UnitWork UnitWork;
        public OrderController(UnitWork unitWork)
        {
            this.UnitWork = unitWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var orders = UnitWork.OrderRepo.GetAll();
            if (orders == null)
                return NotFound();
            else
            {
                List<OrderDTO> orderDTOs = new List<OrderDTO>();
                foreach (Order order in orders)
                {
                    OrderDTO orderDTO = new OrderDTO()
                    {
                        Id = order.Id,
                        RegisterId = order.RegisterId,
                        RegisterName = UnitWork.RegisterRepo.GetById(order.RegisterId).FullName,
                        RegisterEmail = UnitWork.RegisterRepo.GetById(order.RegisterId).Email,
                        Address = order.Address,
                        RegisterPhoneNumber = UnitWork.RegisterRepo.GetById(order.RegisterId).PhoneNumber,
                        TotalPrice = order.TotalPrice,
                        PaymentMethod = order.PaymentMethod,
                        OrderDate =order.OrderDate.ToString(),
                        Status = order.Status

                    };
                    orderDTOs.Add(orderDTO);
                }
                return Ok(orderDTOs);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = UnitWork.OrderRepo.GetById(id);
            if (order == null)
                return NotFound();
            else
            {
                OrderDTO orderDTO = new OrderDTO()
                {
                    Id = order.Id,
                    RegisterName = UnitWork.RegisterRepo.GetById(order.RegisterId).FullName,
                    RegisterEmail = UnitWork.RegisterRepo.GetById(order.RegisterId).Email,
                    Address = order.Address,
                    RegisterPhoneNumber = UnitWork.RegisterRepo.GetById(order.RegisterId).PhoneNumber,
                    TotalPrice = order.TotalPrice,
                    PaymentMethod = order.PaymentMethod,
                    OrderDate = order.OrderDate.ToString(),
                    Status = order.Status
                };
                return Ok(orderDTO);
            }
        }
        [HttpPost]
        public IActionResult Post(OrderDTO orderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (orderDTO.PaymentMethod.Equals("BankTransfer"))
                    {
                        var buyerWallet = UnitWork.db.Wallets.FirstOrDefault(w => w.UserId == orderDTO.RegisterId);
                        if (buyerWallet == null)
                            return BadRequest("Wallet not found for the user.");

                        if (buyerWallet.Balance < orderDTO.TotalPrice)
                            return BadRequest("Insufficient balance in the wallet.");

                        buyerWallet.Balance -= (decimal)orderDTO.TotalPrice;
                        var now = DateTime.Now;
                        buyerWallet.Transactions.Add(new WalletTransaction()
                        {
                            Amount = (decimal)orderDTO.TotalPrice,
                            TransactionType = TransactionType.Withdrawal.ToString(),
                            TransactionDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second,0),
                            Description = "Order payment",
                            WalletId = buyerWallet.Id,
                            WalletName = UnitWork.db.Wallets.FirstOrDefault(w => w.UserId == orderDTO.RegisterId).UserName


                        });

                        var systemWallet = UnitWork.db.Wallets.FirstOrDefault(w => w.UserName == "Infinity"); // System wallet ID is 2
                        if (systemWallet == null)
                            return BadRequest("System wallet not found.");
                        systemWallet.Balance += (decimal)orderDTO.TotalPrice;
                        systemWallet.Transactions.Add(new WalletTransaction()
                        {
                            Amount = (decimal)orderDTO.TotalPrice,
                            TransactionType = TransactionType.Deposit.ToString(),                          
                            TransactionDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second,0),
                            Description = $"Received payment from user {orderDTO.RegisterName}",
                            WalletId = systemWallet.Id,
                            WalletName = UnitWork.db.Wallets.Where(w => w.UserName == "Infinity").FirstOrDefault().UserName


                        });
                        UnitWork.db.Wallets.Update(buyerWallet);
                        UnitWork.db.Wallets.Update(systemWallet);

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
                        UnitWork.OrderRepo.Add(order);
                        UnitWork.Save();
                        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
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
                        UnitWork.OrderRepo.Add(order);
                        UnitWork.Save();
                        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);

                    }
                }
                else
                {
                    return BadRequest("Unsupported payment method.");
                }
            }
            catch
            {
                return BadRequest("Ooops...An error has been occurred.");
            }
        }
        [HttpPut("{id}")]
        public IActionResult Modify(int id, OrderDTO orderDTO)
        {
            if (ModelState.IsValid)
            {
                var order = UnitWork.OrderRepo.GetById(id);
                if (order == null)
                    return NotFound();
                else
                {
                    order.RegisterId = orderDTO.RegisterId;
                    order.Address = orderDTO.Address;
                    order.TotalPrice = (decimal)orderDTO.TotalPrice;
                    order.PaymentMethod = orderDTO.PaymentMethod;
                    order.OrderDate = orderDTO.OrderDate;
                    order.Status = orderDTO.Status;
                    UnitWork.OrderRepo.Update(order, id);
                    UnitWork.Save();
                    return NoContent();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = UnitWork.OrderRepo.GetById(id);
            if (order == null)
                return NotFound();
            else
            {
                UnitWork.OrderRepo.Delete(id);
                UnitWork.Save();
                return NoContent();
            }
        }

    }
}
