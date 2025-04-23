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
    public class FolderImageConfiguer : IEntityTypeConfiguration<FolderImage>
    {
        public void Configure(EntityTypeBuilder<FolderImage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Data).IsRequired();
            builder.HasOne(i=>i.Register).WithMany(r=>r.FolderImages)
                .HasForeignKey(i => i.RegisterId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
