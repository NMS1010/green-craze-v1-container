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
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasIndex(u => u.Name).IsUnique();
            builder.HasIndex(u => u.Slug).IsUnique();

            builder.Property(x => x.ParentId).IsRequired(false);

            builder.Property(x => x.Image).IsRequired();

            builder.Property(x => x.Slug).IsRequired();

            builder.Property(x => x.Status).IsRequired();
        }
    }
}