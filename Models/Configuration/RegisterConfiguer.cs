using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Domain;

namespace Models.Configuration
{
    public class RegisterConfiguer : IEntityTypeConfiguration<Register>
    {
        public void Configure(EntityTypeBuilder<Register> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedNever();
            builder.Property(r => r.FullName).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Email).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Password).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Repassword).IsRequired().HasMaxLength(100);
            builder.Property(r => r.PhoneNumber).IsRequired().HasMaxLength(15);
        }
    }
}
