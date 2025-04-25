using E_Commerce_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTO.Category;
using Models.DTO.Product;
using Unit_Of_Work;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {    private readonly CategoryService categoryService;
         public CategoryController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAllCategory();
            if (categories == null)
                return NotFound();
            else
            {
                return Ok(categories);
            }


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           var category = await categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();
            else
            {
                return Ok(category);
            }
        }
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var category = await categoryService.GetCategoryByName(name);
            if (category == null)
                return NotFound();
            else
            {
                return Ok(category);
            }
        }
      

        // Pageination for products by category
        [HttpGet("ProductByCategory")] 
        public async Task<IActionResult> GetProductByCategory(int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {

            try
            {
                var result = await categoryService.GetProductByCategory(categoryId, page, pageSize);
                if (result == null)
                    return NotFound(new { message = "Category not found or has no products." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
    }
}
