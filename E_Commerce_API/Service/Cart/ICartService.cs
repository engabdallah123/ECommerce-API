using Models.DTO.Cart;

namespace E_Commerce_API.Service.Cart
{
    public interface ICartService
    {
        CartDTO GetCart(string userId);
        void AddToCart(string userId, CartItemDTO cartItem);
        void RemoveFromCart(string userId, int productId);
        void ClearCart(string userId);

    }

}
