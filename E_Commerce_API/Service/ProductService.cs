using Microsoft.EntityFrameworkCore;
using Models.DTO.Product;
using Unit_Of_Work;

namespace E_Commerce_API.Service
{
    public class ProductService
    {
        UnitWork unitWork;
        public ProductService(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            var products = await unitWork.ProductRepo.GetAllAsync();
            if (products == null || !products.Any())
            {
                throw new InvalidOperationException("No products found.");
            }
            else
            {
                List<ProductDTO> productDTO = new List<ProductDTO>();
                foreach (var product in products)
                {
                    ProductDTO productDTO1 = new ProductDTO()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        stock = product.Stock,
                        Brand = product.Brand,
                        Rating = product.Rating,
                        ImageUrl = await unitWork.db.Images
                                    .Where(i => i.ProductId == product.Id)
                                    .Select(i => i.ImageUrl)
                                    .ToListAsync()
                    };
                    productDTO.Add(productDTO1);
                }
                return productDTO;
            }
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await unitWork.ProductRepo.GetByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException($"No products found with name {id}.");
            }
            else
            {
                ProductDTO productDTO = new ProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    stock = product.Stock,
                    Brand = product.Brand,
                    Rating = product.Rating,
                    ImageUrl = await unitWork.db.Images
                                .Where(i => i.ProductId == product.Id)
                                .Select(i => i.ImageUrl)
                                .ToListAsync()
                };
                return productDTO;
            }
        }
        public async Task<ProductDTO> GetProductByName(string name)
        {
            var product = await unitWork.ProductRepo.GetByNameAsync(name);
            if (product == null)
            {
               throw new InvalidOperationException($"No products found with name {name}.");
            }
            else
            {
                ProductDTO productDTO = new ProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    stock = product.Stock,
                    Brand = product.Brand,
                    Rating = product.Rating,
                    ImageUrl = await unitWork.db.Images
                                  .Where(i => i.ProductId == product.Id)
                                  .Select(i => i.ImageUrl)
                                  .ToListAsync()
                };
                return productDTO;
            }
        }
    }
}
