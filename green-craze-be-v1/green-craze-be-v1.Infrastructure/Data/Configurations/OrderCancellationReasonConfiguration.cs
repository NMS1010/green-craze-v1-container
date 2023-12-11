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
    public class OrderCancellationReasonConfiguration : IEntityTypeConfiguration<OrderCancellationReason>
    {
        public void Configure(EntityTypeBuilder<OrderCancellationReason> builder)
        {
            builder.HasIndex(u => u.Name).IsUnique();
            builder.Property(x => x.Note).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}