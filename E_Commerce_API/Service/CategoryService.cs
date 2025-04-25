using Microsoft.EntityFrameworkCore;
using Models.DTO.Category;
using Unit_Of_Work;

namespace E_Commerce_API.Service
{
    public class CategoryService
    {
        UnitWork unitWork;
        public CategoryService(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        public async Task<List<CategoryDTO>> GetAllCategory()
        {
            var categories = await unitWork.CategoryRepo.GetAllAsync();
            if (categories == null)
                return null;
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
                return categoryDTOs;
            }
        }
        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await unitWork.CategoryRepo.GetByIdAsync(id);
            if (category == null)
                return null;
            else
            {
                CategoryDTO categoryDTO = new CategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl
                };
                return categoryDTO;
            }
        }
        public async Task<CategoryDTO> GetCategoryByName(string name)
        {
            var category = await unitWork.CategoryRepo.GetByNameAsync(name);
            if (category == null)
                return null;
            else
            {
                CategoryDTO categoryDTO = new CategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl
                };
                return categoryDTO;
            }
        }
        public async Task<PaginatedProductsByCategoryDTO> GetProductByCategory(int categoryId, int page = 1, int pagesize = 10)
        {
            var category = await unitWork.CategoryRepo.GetByIdAsync(categoryId);
            if (category == null)
            {
                return null;
            }

            var query = unitWork.db.Products
                                 .Where(p => p.CategoryId == categoryId);

            var paginatedProducts = await query
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            var productDTOs = new List<ProductCategoryDTO>();
            foreach (var product in paginatedProducts)
            {
                var imageUrls = await unitWork.db.Images
                                            .Where(i => i.ProductId == product.Id)
                                            .Select(i => i.ImageUrl)
                                            .ToListAsync();
                productDTOs.Add(new ProductCategoryDTO
                {
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

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pagesize);
            return new PaginatedProductsByCategoryDTO
            {
                CurrentPage = page,
                PageSize = pagesize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = productDTOs
            };
        }

    }
}
