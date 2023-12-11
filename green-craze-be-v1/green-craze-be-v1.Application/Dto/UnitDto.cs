namespace green_craze_be_v1.Application.Dto
{
    public class UnitDto : BaseAuditableDto<long>
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}