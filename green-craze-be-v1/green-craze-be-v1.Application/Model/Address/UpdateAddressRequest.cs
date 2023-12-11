using System.Text.Json.Serialization;

namespace green_craze_be_v1.Application.Model.Address
{
    public class UpdateAddressRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public long Id { get; set; }

        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public bool IsDefault { get; set; }
        public long ProvinceId { get; set; }
        public long DistrictId { get; set; }
        public long WardId { get; set; }
    }
}