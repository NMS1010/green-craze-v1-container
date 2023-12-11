using green_craze_be_v1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Data.Configurations
{
    public class VariantConfiguration : IEntityTypeConfiguration<Variant>
    {
        public void Configure(EntityTypeBuilder<Variant> builder)
        {
            builder.HasIndex(u => u.Sku).IsUnique();
            builder.Property(x => x.ItemPrice).HasColumnType("DECIMAL").IsRequired();
            builder.Property(x => x.PromotionalItemPrice).HasColumnType("DECIMAL");
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.TotalPrice).HasColumnType("DECIMAL").IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}