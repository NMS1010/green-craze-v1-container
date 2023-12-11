using green_craze_be_v1.Application.Model.Role;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace green_craze_be_v1.Application.Specification.Role
{
    public class RoleSpecification : BaseSpecification<AppRole>
    {
        public RoleSpecification(string name) : base(x => x.NormalizedName.ToLower() == name)
        {
        }

        public RoleSpecification(GetRolePagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name.Contains(keyword);
            }

            if (string.IsNullOrEmpty(request.ColumnName))
                request.ColumnName = "Id";
            AddSorting(request.ColumnName, request.IsSortAscending);

            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }
    }
}