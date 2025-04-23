using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Domain;

namespace Models.Configuration
{
    public class OrderConfiguer : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.RegisterId).IsRequired();
            builder.Property(o => o.RegisterName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.RegisterEmail).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Address).IsRequired().HasMaxLength(200);
            builder.Property(o => o.PhoneNumber).IsRequired().HasMaxLength(15);

            builder.Property(o => o.Status).IsRequired().HasMaxLength(50);
            builder.Property(o => o.PaymentMethod).IsRequired().HasMaxLength(50);

            // Navigation properties Relationships
            builder.HasOne(o => o.Register)
                .WithMany(r => r.Orders) 
                .HasForeignKey(o => o.RegisterId)
                .OnDelete(DeleteBehavior.NoAction); // Optional: specify delete behavior

           
        }
    }

}
