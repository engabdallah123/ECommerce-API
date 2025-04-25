using Models.DTO.Cart;

namespace E_Commerce_API.Service.Cart
{
    public class  InMemoryCartService : ICartService
    {
        private readonly Dictionary<string, CartDTO> _carts = new Dictionary<string, CartDTO>(); // In-memory storage for carts similar to a database
        public CartDTO GetOrCreateCart(string userId) // Method to get or create a cart for a user
        {
            if(!_carts.ContainsKey(userId))
            {
                _carts[userId] = new CartDTO();
            }
            return _carts[userId];
        }
        public CartDTO GetCart(string userId)
        {
            var cart = GetOrCreateCart(userId);

            return new CartDTO
            {
                Items = cart.Items.Select(i => new CartItemDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity)

            };


        }
        public void AddToCart(string userId, CartItemDTO cartItem)
        {
            var cart = GetOrCreateCart(userId);
            var item = new CartItemDTO
            {
                ProductId = cartItem.ProductId,
                ProductName = cartItem.ProductName,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };
            cart.Items.Add(item);

            

        }
        public void RemoveFromCart(string userId, int productId)
        {
            var cart = GetOrCreateCart(userId);
           cart.Items.RemoveAll(i => i.ProductId == productId);

        }
        public void ClearCart(string userId)
        {
          _carts.Remove(userId);
        }

    }

}
