using Data;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Domain;
using Repositry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Of_Work
{
   public class UnitWork
    {
       public AppDbContext db;
        GenericRepositry<Product> productRepo;
        GenericRepositry<Register> registerRepo;       
        GenericRepositry<Category> categoryRepo;
        GenericRepositry<Order> orderRepo;
        GenericRepositry<Image> imageRepo;
        GenericRepositry<OrderItem> orderItemRepo;
        GenericRepositry<Review> reviewRepo;
        GenericRepositry<Wallet> walletRepo;
        GenericRepositry<WalletTransaction> walletTransactionRepo;
        GenericRepositry<FolderImage> folderImageRepo;
        GenericRepositry<CustomerFeedback> customerFeedbackRepo;

        public UnitWork(AppDbContext db)
        {
            this.db = db;
        }

        public GenericRepositry<Product> ProductRepo
        {
            get
            {
                if (productRepo == null)
                {
                    productRepo = new GenericRepositry<Product>(db);
                }
                return productRepo;
            }
        }
        public GenericRepositry<Register> RegisterRepo
        {
            get
            {
                if (registerRepo == null)
                {
                    registerRepo = new GenericRepositry<Register>(db);
                }
                return registerRepo;
            }
        }
      
        public GenericRepositry<Category> CategoryRepo
        {
            get
            {
                if (categoryRepo == null)
                {
                    categoryRepo = new GenericRepositry<Category>(db);
                }
                return categoryRepo;
            }
        }
        public GenericRepositry<Order> OrderRepo
        {
            get
            {
                if (orderRepo == null)
                {
                    orderRepo = new GenericRepositry<Order>(db);
                }
                return orderRepo;
            }
        }      
        public GenericRepositry<OrderItem> OrderItemRepo
        {
            get
            {
                if (orderItemRepo == null)
                {
                    orderItemRepo = new GenericRepositry<OrderItem>(db);
                }
                return orderItemRepo;
            }
        }      
        public GenericRepositry<Image> ImageRepo
        {
            get
            {
                if (imageRepo == null)
                {
                    imageRepo = new GenericRepositry<Image>(db);
                }
                return imageRepo;
            }
        }
        public GenericRepositry<Review> ReviewRepo
        {
            get
            {
                if (reviewRepo == null)
                {
                    reviewRepo = new GenericRepositry<Review>(db);
                }
                return reviewRepo;
            }
        }
        public GenericRepositry<Wallet> WalletRepo
        {
            get
            {
                if (walletRepo == null)
                {
                    walletRepo = new GenericRepositry<Wallet>(db);
                }
                return walletRepo;
            }
        }
        public GenericRepositry<WalletTransaction> WalletTransactionRepo
        {
            get
            {
                if (walletTransactionRepo == null)
                {
                    walletTransactionRepo = new GenericRepositry<WalletTransaction>(db);
                }
                return walletTransactionRepo;
            }
        }
        public GenericRepositry<FolderImage> FolderImageRepo
        {
            get
            {
                if (folderImageRepo == null)
                {
                    folderImageRepo = new GenericRepositry<FolderImage>(db);
                }
                return folderImageRepo;
            }
        }
        public GenericRepositry<CustomerFeedback> CustomerFeedbackRepo
        {
            get
            {
                if (customerFeedbackRepo == null)
                {
                    customerFeedbackRepo = new GenericRepositry<CustomerFeedback>(db);
                }
                return customerFeedbackRepo;
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Dispose()
        {
            db.Dispose();
        }
        // transactions
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }

    }
}
