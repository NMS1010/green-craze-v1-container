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
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Image).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.PromotionalPercent).IsRequired();
            builder.Property(x => x.Slug).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}