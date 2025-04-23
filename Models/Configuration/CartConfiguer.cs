using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Domain;
using System.Linq;

namespace Models.Configuration
{
    public class CartConfiguer : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.ProductId).IsRequired();
            builder.Property(c => c.Quantity).IsRequired();

            builder.Property(c => c.TotalPrice).HasColumnType("decimal(18,2)");
            builder.Property(c => c.TotalPrice).IsRequired();

            // Navigation properties Relationships
            builder.HasOne(c => c.Product)
                .WithMany(p => p.Carts)
                  .HasForeignKey(c => c.ProductId)
                   .OnDelete(DeleteBehavior.NoAction);

                builder.HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }


}
