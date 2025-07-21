using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
    public class UserSettings
    {
        [Key]
        public int SettingsId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public bool EmailNotifications { get; set; }

        [Required]
        public bool SMSNotifications { get; set; }

        [Required]
        public bool PushNotifications { get; set; }

        [StringLength(10)]
        public string Language { get; set; }

        [StringLength(10)]
        public string Currency { get; set; }

        [StringLength(50)]
        public string TimeZone { get; set; }

        [Required]
        public bool TermsAccepted { get; set; }

        public User User { get; set; }
    }
}
