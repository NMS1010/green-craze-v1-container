using System.Text.Json.Serialization;

namespace green_craze_be_v1.Application.Model.Auth
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }

        [JsonIgnore]
        public string Type { get; set; }
    }
}