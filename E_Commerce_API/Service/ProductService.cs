using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Models.Domain;
using Models.DTO.Image;
using Models.DTO.Product;
using System.Diagnostics;
using Unit_Of_Work;

namespace E_Commerce_API.Service
{
    public class ProductService
    {
        UnitWork unitWork;
        private readonly ILogger<ProductService> logger;
        private readonly IMemoryCache cache;

        public ProductService(UnitWork unitWork,ILogger<ProductService> logger,
            IMemoryCache cache)
        {
            this.unitWork = unitWork;
            this.logger = logger;
            this.cache = cache;
        }
        public async Task<List<ProductDTO>> GetAllProducts()
        {
            var stopWatch = Stopwatch.StartNew();

            var cacheKey = "all_product";
            if(!cache.TryGetValue(cacheKey, out List<ProductDTO> cachedProducts))
            {
                logger.LogInformation("Cache Is Empty..loading products from database");
                try
                {
                    var products = await unitWork.ProductRepo.GetAllAsync();

                    List<ProductDTO> productsDTO = new List<ProductDTO>();
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
                            // FIX: Remove await, since FirstOrDefault returns string synchronously
                            CatName = await unitWork.db.Products
                                .Where(i => i.Id == product.Id)
                                .Select(c => c.Category.Name)
                                .FirstOrDefaultAsync(),
                            CatId = await unitWork.db.Products
                                 .Where(i => i.Id == product.Id)
                                 .Select(c => c.Category.Id)
                                 .FirstOrDefaultAsync(),
                            ImageUrl = await unitWork.db.Images
                                        .Where(i => i.ProductId == product.Id)
                                        .Select(i => i.ImageUrl)
                                        .ToListAsync()
                        };
                        productsDTO.Add(productDTO1);

                    }
                    cachedProducts = productsDTO;

                    var entryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                    cache.Set(cacheKey,cachedProducts, entryOptions);

                    logger.LogInformation("Products have been cached");
                    return cachedProducts;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while retrieving products from database");
                }
                
            }
            else
            {
                logger.LogInformation("Retrieved {Count} products from cache", cachedProducts.Count);
                
            }

            stopWatch.Stop();

            logger.LogInformation("Products retrieved in {ElapsedMs} ms", stopWatch.ElapsedMilliseconds);
            
            return cachedProducts;
           
        }
        public async Task<List<ProductDTO>> GetProductMoreRating()
        {
            var products = await unitWork.db.Products
                           .Where(p => p.Rating == 5).ToListAsync();
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
                        // FIX: Remove await, since FirstOrDefault returns string synchronously
                        CatName = await unitWork.db.Products
                            .Where(i => i.Id == product.Id)
                            .Select(c => c.Category.Name)
                            .FirstOrDefaultAsync(),
                        CatId = await unitWork.db.Products
                             .Where(i => i.Id == product.Id)
                             .Select(c => c.Category.Id)
                             .FirstOrDefaultAsync(),
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
                logger.LogWarning("No product found with #{Id}.", id);
                return null;
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
                    CatName = await unitWork.db.Products
                            .Where(i => i.Id == product.Id)
                            .Select(c => c.Category.Name)
                            .FirstOrDefaultAsync(),
                    CatId = await unitWork.db.Products
                             .Where(i => i.Id == product.Id)
                             .Select(c => c.Category.Id)
                             .FirstOrDefaultAsync(),

                    ImageUrl = await unitWork.db.Images
                                .Where(i => i.ProductId == product.Id)
                                .Select(i => i.ImageUrl)
                                .ToListAsync()
                };
                return productDTO;
            }
        }
        public async Task<List<ProductDTO>> GetProductByName(string name)
        {
            var product = await unitWork.ProductRepo.GetByNameAsync(name);
            if (product == null)
            {
                throw new InvalidOperationException($"No products found with name {name}.");
            }
            else
            {
                List<ProductDTO> productDTO = new List<ProductDTO>();
                foreach (var products in product)
                {
                    ProductDTO productDTO1 = new ProductDTO()
                    {
                        Id = products.Id,
                        Name = products.Name,
                        Description = products.Description,
                        Price = products.Price,
                        stock = products.Stock,
                        Brand = products.Brand,
                        Rating = products.Rating,
                        CatName = await unitWork.db.Products
                            .Where(i => i.Name == products.Name)
                            .Select(c => c.Category.Name)
                            .FirstOrDefaultAsync(),
                        CatId = await unitWork.db.Products
                             .Where(i => i.Id == products.Id)
                             .Select(c => c.Category.Id)
                             .FirstOrDefaultAsync(),
                        ImageUrl = await unitWork.db.Images
                                    .Where(i => i.ProductId == products.Id)
                                    .Select(i => i.ImageUrl)
                                    .ToListAsync()
                    };
                    productDTO.Add(productDTO1);
                }
                return productDTO;
            }
        }

        public void EditeProduct(int id, ProductDTO productDTO)
        {
            var product = unitWork.ProductRepo.GetById(id);
            if (product == null)
                throw new InvalidOperationException($"No products found with id {id}.");
            else
            {
                product.Brand = productDTO.Brand ?? product.Brand;
                product.Price = productDTO.Price ?? product.Price;
                product.Name = productDTO.Name ?? product.Name;
                product.Stock = productDTO.stock ?? product.Stock;
                product.Rating = productDTO.Rating ?? product.Rating;
                product.Description = productDTO.Description ?? product.Description;
                product.CategoryId = productDTO.CatId ?? product.CategoryId;


            }
            unitWork.ProductRepo.Update(product, id);
            unitWork.ProductRepo.Save();


        }
        public void DeleteProduct(int id)
        {
            var product = unitWork.ProductRepo.GetById(id);
            if (product == null)
                throw new InvalidOperationException($"No products found with id {id}.");
            else
            {
                unitWork.ProductRepo.Delete(id);
                unitWork.ProductRepo.Save();
            }
        }
        public async Task<int> AddProductAsync(AddProductDTO dto)
        {
            var product = new Product
            {
                Price = dto.Price ?? 0,
                Name = dto.Name ?? "Unknown",
                Description = dto.Description,
                Stock = dto.stock ?? 0,
                Rating = dto.Rating ?? 0,
                Brand = dto.Brand,
                CategoryId = dto.CatId ?? 1
            };

            await unitWork.ProductRepo.AddAsync(product);
            await unitWork.ProductRepo.SaveAsync();

            return product.Id; 
        }


        public async Task<List<ImageDTO>> UploadImagesAsync(UploadImageDTO dto)
        {
            if (dto.Files == null || dto.Files.Count == 0)
                throw new ArgumentException("No files uploaded");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var result = new List<ImageDTO>();

            foreach (var file in dto.Files)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var image = new Image
                    {
                        ProductId = dto.ProductId,
                        ImageUrl = "/uploads/products/" + fileName
                    };

                    await unitWork.ImageRepo.AddAsync(image);
                    await unitWork.ImageRepo.SaveAsync();

                    result.Add(new ImageDTO
                    {
                        Id = image.Id,
                        ProductId = image.ProductId,
                        ImageUrl = image.ImageUrl
                    });
                }
            }

            return result;
        }



    }
}
