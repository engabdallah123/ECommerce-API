using E_Commerce_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTO.Image;
using Models.DTO.Product;
using Models.DTO.Wallet;
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
        [HttpGet("Rating")]
        public async Task<IActionResult> GetRatingProducts()
        {
            var products = await productService.GetProductMoreRating();
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
                return Ok(new { data = product });
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
                return Ok(new { data = product });
            }

        }
        [HttpPut("{id}")]
        public IActionResult EditeProduct(int id,[FromBody]ProductDTO productDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    productService.EditeProduct(id, productDTO);
                    return Ok(new { message = "Edited successfully" });

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDTO dto)
        {
            var productId = await productService.AddProductAsync(dto);
            return Ok(new { Message = "Product added successfully", ProductId = productId });
        }
        [HttpPost("upload-photo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImages([FromForm] UploadImageDTO dto)
        {
            var images = await productService.UploadImagesAsync(dto);
            return Ok(new { Message = "Images uploaded successfully", Images = images });
        }

    }
}
