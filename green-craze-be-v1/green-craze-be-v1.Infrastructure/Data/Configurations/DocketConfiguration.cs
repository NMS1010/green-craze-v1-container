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
    public class DocketConfiguration : IEntityTypeConfiguration<Docket>
    {
        public void Configure(EntityTypeBuilder<Docket> builder)
        {
            builder.Property(u => u.Type).IsRequired();
            builder.Property(u => u.Code).IsRequired();
            builder.HasIndex(u => u.Code).IsUnique();
            builder.Property(u => u.Quantity).IsRequired();
        }
    }
}
