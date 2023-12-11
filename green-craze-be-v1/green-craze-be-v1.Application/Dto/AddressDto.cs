namespace green_craze_be_v1.Application.Dto
{
    public class AddressDto : BaseAuditableDto<long>
    {
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public bool IsDefault { get; set; }
        public bool Status { get; set; }
        public ProvinceDto Province { get; set; }
        public DistrictDto District { get; set; }
        public WardDto Ward { get; set; }
    }
}