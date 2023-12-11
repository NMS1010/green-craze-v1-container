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
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasIndex(u => u.Name).IsUnique();
            builder.HasIndex(u => u.Code).IsUnique();
            builder.Property(x => x.Image).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}