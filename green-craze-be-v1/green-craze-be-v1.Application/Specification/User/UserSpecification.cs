using green_craze_be_v1.Application.Model.User;

namespace green_craze_be_v1.Application.Specification.User
{
    public class UserSpecification : BaseSpecification<Domain.Entities.AppUser>
    {
        public UserSpecification(GetUserPagingRequest request, bool isPaging = false)
        {
            var key = request.Search;
            if (!string.IsNullOrEmpty(key))
            {
                Criteria = x => x.Staff == null && x.Email.ToLower().Contains(key)
                || x.PhoneNumber.ToLower().Contains(key)
                || x.Gender.ToLower().Contains(key)
                || x.Dob.Value.ToString().Contains(key)
                || x.FirstName.ToLower().Contains(key)
                || x.LastName.ToLower().Contains(key);
            }
            else
            {
                Criteria = x => x.Staff == null;
            }
            AddInclude(x => x.Staff);

            if (string.IsNullOrEmpty(request.ColumnName))
                request.ColumnName = "Id";
            AddSorting(request.ColumnName, request.IsSortAscending);

            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }

        public UserSpecification(string userId) : base(x => x.Id == userId)
        {
            AddInclude(x => x.Addresses);
            AddInclude(x => x.Cart);
            AddInclude(x => x.AppUserTokens);
        }

        public UserSpecification()
        {
            AddInclude(x => x.AppUserTokens);
        }

        public UserSpecification(string email, bool test = false) : base(x => x.Email == email)
        {
            AddInclude(x => x.AppUserTokens);
        }
    }
}