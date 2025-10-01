using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTO.Cart;
using Unit_Of_Work;

namespace E_Commerce_API.Service
{
    public class CartServices
    {
        private readonly UnitWork unitWork;

        public CartServices(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            var cart = await unitWork.db.Cart.Where(i => i.UserId == userId).FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                unitWork.CartRepo.Add(cart);
                await unitWork.SaveAsync();

       
            }

            return new CartDTO
            {
                userId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDTO
                {
                    ProductId = ci.ProductId,                    
                   Price = unitWork.db.Products.Where(i => i.Id == ci.ProductId ).Select(p => p.Price).FirstOrDefault(),
                    Quantity = ci.Quantity
                }).ToList()
            };
        }

        public async Task AddToCartAsync(string userId,CartItemDTO itemDTO)
        {
            var cart = await unitWork.db.Cart.Where(i => i.UserId == userId).FirstOrDefaultAsync();
                

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                unitWork.CartRepo.Add(cart);
                await unitWork.SaveAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == itemDTO.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += itemDTO.Quantity;
                unitWork.CartItemsRepo.Updateing(cartItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = itemDTO.ProductId,
                    Quantity = itemDTO.Quantity,
                    CartId = cart.Id,
                    
                };
                await unitWork.CartItemsRepo.AddAsync(newItem);
            }
            await unitWork.SaveAsync();
        }

        public async Task RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await unitWork.db.Cart.Where(i => i.UserId == userId).FirstOrDefaultAsync();
            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                unitWork.CartItemsRepo.Delete(cartItem.Id);
                await unitWork.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException("Product not found in cart.");
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await unitWork.db.Cart.Where(i => i.UserId == userId).FirstOrDefaultAsync();

            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            foreach (var item in cart.CartItems.ToList())
            {
                unitWork.CartItemsRepo.Delete(item.Id);
            }

            await unitWork.SaveAsync();
        }

        public async Task UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            var cart = await unitWork.db.Cart.Where(i => i.UserId == userId).FirstOrDefaultAsync();

            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                unitWork.CartItemsRepo.Updateing(cartItem);
                await unitWork.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException("Product not found in cart.");
            }
        }
    }

}
