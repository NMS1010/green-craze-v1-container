namespace green_craze_be_v1.Application.Dto
{
    public class BrandDto : BaseAuditableDto<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }
        public bool Status { get; set; }
    }
}
