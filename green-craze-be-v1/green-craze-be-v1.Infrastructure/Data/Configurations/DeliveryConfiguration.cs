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
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasIndex(u => u.Name).IsUnique();
            builder.Property(x => x.Price).HasColumnType("DECIMAL");
            builder.Property(x => x.Image).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}