using System.Text.Json.Serialization;

namespace green_craze_be_v1.Application.Model.Cart
{
    public class UpdateCartItemRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }

        [JsonIgnore]
        public long CartItemId { get; set; }

        public int Quantity { get; set; }
    }
}