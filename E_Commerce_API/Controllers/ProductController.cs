using E_Commerce_API.Service;
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
   // [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;
        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProducts();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            else
            {
                return Ok(products);
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"No products found with id {id}.");
            }
            else
            {
                return Ok(product);
            }
        }
        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchProduct(string name)
        {
            var product = await productService.GetProductByName(name);
            if (product == null)
            {
                return NotFound($"No products found with name {name}.");
            }
            else
            {
                return Ok(product);
            }

        }
    }
}
