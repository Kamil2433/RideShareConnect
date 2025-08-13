using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public string ProfilePicture { get; set; }

        public User User { get; set; }
    }
}