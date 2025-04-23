using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configuration
{
   public  class OrderProductConfiguer : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(op => new { op.ProductId, op.OrderId });
            builder.Property(op => op.Quantity).IsRequired();
            builder.Property(op => op.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
            // Relationships
            builder.HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.NoAction); // Optional: specify delete behavior
            builder.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.NoAction); // Optional: specify delete behavior
        }
    }
  
}
