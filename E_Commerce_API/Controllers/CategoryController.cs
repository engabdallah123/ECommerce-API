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
    {
        UnitWork unitWork;
        public CategoryController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await unitWork.CategoryRepo.GetAllAsync();
            if (categories == null)
                return NotFound();
            else
            {
                List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
                foreach (var category in categories)
                {
                    CategoryDTO categoryDTO = new CategoryDTO()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description,
                        ImageUrl = category.ImageUrl
                    };
                    categoryDTOs.Add(categoryDTO);
                }
                return Ok(categoryDTOs);
            }


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await unitWork.CategoryRepo.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            else
            {
                CategoryDTO categoryDTO = new CategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl
                };
                return Ok(categoryDTO);
            }
        }
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var category = await unitWork.CategoryRepo.GetByNameAsync(name);
            if (category == null)
                return NotFound();
            else
            {
                CategoryDTO categoryDTO = new CategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl
                };
                return Ok(categoryDTO);
            }
        }
      

        // Pageination for products by category
        [HttpGet("ProductByCategory")] //api/Category/ProductByCategory?categoryId=1&page=1&pageSize=10
        public async Task<IActionResult> GetProductByCategory(int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var category = await unitWork.CategoryRepo.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return NotFound($"Category with ID {categoryId} not found.");
                }

                var query = unitWork.db.Products   // حدد المنتجات اللي في ال كاتيجوري
                                 .Where(p => p.CategoryId == categoryId);

                var paginatedProducts = await query  // المنتجات دي اعملها ال باجينيشن وحطها في ليست اسينك
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
               
                var productDTOs = new List<ProductCategoryDTO>();    //  هنا بقي قولي انت هتعمل بيهم ايه واعملهم دي تي او عشان اللوب المالانهايه         
                foreach(var product in paginatedProducts)
                {
                    var imageUrls= await unitWork.db.Images   // عشان اجيبهم في ليست اسينك
                                            .Where(i => i.ProductId == product.Id)
                                            .Select(i => i.ImageUrl)
                                            .ToListAsync();
                    productDTOs.Add(new ProductCategoryDTO {


                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Brand = product.Brand,
                        Rating = product.Rating,
                        Stock = product.Stock,
                        ImageUrl = imageUrls,
                        Category = new CategoryDTO
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Description = category.Description,
                            ImageUrl = category.ImageUrl
                        }


                    });

                }

                var totalCount = await query.CountAsync(); // علشان ما تجيبش كل الداتا من الأول وتعد.
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                return Ok(new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize,
                    Data = productDTOs
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
