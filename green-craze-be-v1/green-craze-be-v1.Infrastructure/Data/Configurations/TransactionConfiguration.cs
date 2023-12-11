using green_craze_be_v1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasIndex(u => u.PaypalOrderId).IsUnique();
            builder.Property(x => x.TotalPay).HasColumnType("DECIMAL").IsRequired();
            builder.Property(x => x.PaymentMethod).IsRequired();
        }
    }
}