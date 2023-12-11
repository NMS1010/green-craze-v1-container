using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Data.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            var now = DateTime.Now;
            builder.HasData(USER_ROLE.Roles.Select(x => new AppRole()
            {
                Name = x,
                CreatedAt = now,
                CreatedBy = "System",
                NormalizedName = x
            }));
        }
    }
}