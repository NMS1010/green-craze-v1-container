using Microsoft.AspNetCore.Identity;

namespace green_craze_be_v1.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public Cart Cart { get; set; }
        public Staff Staff { get; set; }
        public ICollection<UserFollowProduct> UserFollowProducts { get; set; } = new List<UserFollowProduct>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<AppUserToken> AppUserTokens { get; set; } = new List<AppUserToken>();
    }
}