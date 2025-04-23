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
   public class WalletTransactionConfiguer : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.HasKey(wt => wt.Id);
            builder.Property(wt => wt.Amount).HasColumnType("decimal(18,2)");
            builder.Property(wt => wt.TransactionDate).HasDefaultValueSql("GETDATE()");
            builder.Property(wt => wt.TransactionType).IsRequired();
            builder.Property(wt => wt.Description).IsRequired(false);
            builder.Property(w => w.WalletName).HasMaxLength(50);
            builder.HasOne(wt => wt.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(wt => wt.WalletId);
        }
    }
   
}
