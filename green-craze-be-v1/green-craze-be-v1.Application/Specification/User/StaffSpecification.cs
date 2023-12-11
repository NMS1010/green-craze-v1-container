using green_craze_be_v1.Application.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.User
{
    public class StaffSpecification : BaseSpecification<Domain.Entities.Staff>
    {
        public StaffSpecification(GetUserPagingRequest request, bool isPaging = false)
        {
            AddInclude(x => x.User);
            var key = request.Search;
            if (!string.IsNullOrEmpty(key))
            {
                Criteria = x => x.User.Email.ToLower().Contains(key)
                || x.User.FirstName.ToLower().Contains(key)
                || x.User.LastName.ToLower().Contains(key)
                || x.Type.ToLower().Contains(key);
            }
            var columnName = request.ColumnName.ToLower();
            if (request.IsSortAscending)
            {
                if (columnName == nameof(Domain.Entities.AppUser.Email).ToLower())
                {
                    AddOrderBy(x => x.User.Email);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.CreatedAt).ToLower())
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.FirstName).ToLower())
                {
                    AddOrderBy(x => x.User.FirstName);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.LastName).ToLower())
                {
                    AddOrderBy(x => x.User.LastName);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.Status).ToLower())
                {
                    AddOrderBy(x => x.User.Status);
                }
                else if (columnName == nameof(Domain.Entities.Staff.Type).ToLower())
                {
                    AddOrderBy(x => x.Type);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (columnName == nameof(Domain.Entities.AppUser.Email).ToLower())
                {
                    AddOrderByDescending(x => x.User.Email);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.CreatedAt).ToLower())
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.FirstName).ToLower())
                {
                    AddOrderByDescending(x => x.User.FirstName);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.LastName).ToLower())
                {
                    AddOrderByDescending(x => x.User.LastName);
                }
                else if (columnName == nameof(Domain.Entities.AppUser.Status).ToLower())
                {
                    AddOrderByDescending(x => x.User.Status);
                }
                else if (columnName == nameof(Domain.Entities.Staff.Type).ToLower())
                {
                    AddOrderByDescending(x => x.Type);
                }
                else
                {
                    AddOrderByDescending(x => x.Id);
                }
            }
            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }

        public StaffSpecification(long staffId) : base(x => x.Id == staffId)
        {
            AddInclude(x => x.User);
        }
    }
}