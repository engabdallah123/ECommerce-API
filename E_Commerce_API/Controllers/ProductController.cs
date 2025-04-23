using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTO.Product;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        UnitWork unitWork;
        public ProductController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await unitWork.ProductRepo.GetAllAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
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
                return Ok(productDTO);

            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await unitWork.ProductRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
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
                return Ok(productDTO);
            }
        }
        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchProducts(string name)
        {
            var product = await unitWork.ProductRepo.GetByNameAsync(name);
            if (product == null)
            {
                return NotFound($"No products found with name {name}.");
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
                return Ok(productDTO);
            }
        }
    }
}
