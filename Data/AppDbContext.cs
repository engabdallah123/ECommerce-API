using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Configuration;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
       
        public DbSet<Register> Registers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }       
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<FolderImage> FolderImages { get; set; }
       public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }

        public AppDbContext(DbContextOptions dbContext) : base(dbContext)
        {
            
        }
        public AppDbContext()
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<Wallet>(builder =>
            {
                builder.HasOne(w => w.Register)
                  .WithOne(r => r.Wallet)
                     .HasForeignKey<Wallet>(w => w.UserId)
                      .IsRequired(); // system wallet => UserId = 2
            });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            

        }
    }
}