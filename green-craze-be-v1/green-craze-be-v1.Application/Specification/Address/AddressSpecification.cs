using green_craze_be_v1.Application.Model.Address;

namespace green_craze_be_v1.Application.Specification.Address
{
    public class AddressSpecification : BaseSpecification<Domain.Entities.Address>
    {
        public AddressSpecification(string userId) : base(x => x.User.Id == userId)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Province);
            AddInclude(x => x.District);
            AddInclude(x => x.Ward);
        }

        public AddressSpecification(GetAddressPagingRequest request, bool isPaging = false)
        {
            if (request.Status)
            {
                Criteria = x => x.Status == true && x.User.Id == request.UserId;
            }
            else
            {
                Criteria = x => x.User.Id == request.UserId;
            }
            AddInclude(x => x.User);
            AddInclude(x => x.Province);
            AddInclude(x => x.District);
            AddInclude(x => x.Ward);
            AddOrderByDescending(x => x.IsDefault);
            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }

        public AddressSpecification(string userId, long id)
            : base(x => x.User.Id == userId && x.Id == id)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Province);
            AddInclude(x => x.District);
            AddInclude(x => x.Ward);
            AddOrderByDescending(x => x.IsDefault);
        }

        public AddressSpecification(string userId, bool isDefault)
            : base(x => x.User.Id == userId && x.IsDefault == isDefault)
        {
            AddInclude(x => x.User);
            AddInclude(x => x.Province);
            AddInclude(x => x.District);
            AddInclude(x => x.Ward);
        }
    }
}