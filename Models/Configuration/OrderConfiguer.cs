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

            builder.Property(o => o.OrderStatus)
                .HasDefaultValue("Pending");

            builder.HasMany(o => o.Items)
          .WithOne(oi => oi.Order)
          .HasForeignKey(oi => oi.OrderId)
          .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.User)
            .WithMany(r => r.Orders)
             .HasForeignKey(o => o.UserId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.CustomerInfo)
                .WithMany(c => c.Orders)
                .HasForeignKey(f => f.CustId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }

}
