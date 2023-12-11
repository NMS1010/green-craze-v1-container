using System.Text.Json.Serialization;

namespace green_craze_be_v1.Application.Model.Notification
{
    public class UpdateNotificationRequest
    {
        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}