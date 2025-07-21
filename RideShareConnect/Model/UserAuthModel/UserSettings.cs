using System.ComponentModel.DataAnnotations;

namespace RideShareConnect.Models
{
    public class UserSettings
    {
        [Key]
        public int SettingsId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public bool TermsAccepted { get; set; }

        public bool EmailNotifications { get; set; }

        public bool SMSNotifications { get; set; }

        public bool PushNotifications { get; set; }

        [StringLength(10)]
        public string Language { get; set; }

        [StringLength(10)]
        public string Currency { get; set; }

        [StringLength(50)]
        public string TimeZone { get; set; }

        public User User { get; set; }
    }
}