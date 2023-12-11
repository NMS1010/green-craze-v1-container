using System.Text.Json.Serialization;

namespace green_craze_be_v1.Application.Model.Cart
{
    public class CreateCartItemRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public long VariantId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}