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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(u => u.Code).IsUnique();
            builder.Property(x => x.ShippingCost).HasColumnType("DECIMAL").IsRequired();
            builder.Property(x => x.TotalAmount).HasColumnType("DECIMAL").IsRequired();
            builder.Property(x => x.DeliveryMethod).IsRequired();
            builder.Property(x => x.PaymentStatus).IsRequired();
            builder.Property(x => x.Tax).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}