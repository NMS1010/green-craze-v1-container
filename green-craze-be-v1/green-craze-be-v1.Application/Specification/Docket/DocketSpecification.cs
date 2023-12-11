namespace green_craze_be_v1.Application.Specification.Docket
{
    public class DocketSpecification : BaseSpecification<Domain.Entities.Docket>
    {
        public DocketSpecification(long productId) : base(x => x.Product.Id == productId)
        {
            AddInclude(x => x.Order);
            AddInclude(x => x.Product);
        }

        public DocketSpecification(string type) : base(x => x.Type == type)
        {
            AddInclude(x => x.Order);
            AddInclude(x => x.Product);
        }

        public DocketSpecification(string type, DateTime firstDate, DateTime lastDate) 
            : base(x => x.Type == type && x.CreatedAt >= firstDate && x.CreatedAt <= lastDate)
        {
            AddInclude(x => x.Order);
            AddInclude(x => x.Product);
        }
    }
}