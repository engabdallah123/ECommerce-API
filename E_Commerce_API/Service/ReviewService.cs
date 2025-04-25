using Models.Domain;
using Models.DTO.Review;
using Unit_Of_Work;

namespace E_Commerce_API.Service
{
    public class ReviewService
    {
        UnitWork unitWork;
        public ReviewService(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        public async Task<ReviewDTO> GetById(int id)
        {
            var review = await unitWork.ReviewRepo.GetByIdAsync(id);
            if (review == null)
                throw new Exception("Review not found");
            else
            {
                ReviewDTO reviewDTO = new ReviewDTO()
                {
                    Id = review.Id,
                    Subject = review.Subject,
                    Comment = review.Comment,
                    UserName = unitWork.db.Registers
                                .Where(u => u.Id == review.UserId)
                                .Select(u => u.FullName)
                                .FirstOrDefault(),
                    ProductName = unitWork.db.Products
                                .Where(p => p.Id == review.ProductId)
                                .Select(p => p.Name)
                                .FirstOrDefault()
                };
                return reviewDTO;
            }

        }
        public void Post(ReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
                throw new Exception("Review is null");
            if (string.IsNullOrEmpty(reviewDTO.Subject) || string.IsNullOrEmpty(reviewDTO.Comment))
                throw new Exception("Subject or Comment is empty");
            Review review = new Review()
            {
                Subject = reviewDTO.Subject,
                Comment = reviewDTO.Comment,
                UserId = unitWork.db.Registers
                            .Where(u => u.FullName == reviewDTO.UserName)
                            .Select(u => u.Id)
                            .FirstOrDefault(),
                ProductId = unitWork.db.Products
                            .Where(p => p.Name == reviewDTO.ProductName)
                            .Select(p => p.Id)
                            .FirstOrDefault()
            };
            unitWork.ReviewRepo.Add(review);
            unitWork.Save();
        }
        public void Update(ReviewDTO reviewDTO, int id)
        {
            var review = unitWork.ReviewRepo.GetById(id);
            if (review == null)
                throw new Exception("Review not found");
            else
            {
                Review review1 = new Review()
                {
                    Id = review.Id,
                    Subject = review.Subject,
                    Comment = review.Comment,
                    UserId = unitWork.db.Registers
                            .Where(u => u.FullName == reviewDTO.UserName)
                            .Select(u => u.Id)
                            .FirstOrDefault(),
                    ProductId = unitWork.db.Products
                            .Where(p => p.Name == reviewDTO.ProductName)
                            .Select(p => p.Id)
                            .FirstOrDefault()
                };

                unitWork.ReviewRepo.Update(review, id);
                unitWork.Save();

            }
        }
        public void Delete(int id)
        {
            var review = unitWork.ReviewRepo.GetById(id);
            if (review == null)
                throw new Exception("Review not found");
            else
            {
                unitWork.ReviewRepo.Delete(id);
                unitWork.Save();
            }
        }
    }
}
