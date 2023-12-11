using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Domain.Entities.Address>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Address> builder)
        {
            builder.Property(x => x.Street).IsRequired();
            builder.Property(x => x.Receiver).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
        }
    }
}