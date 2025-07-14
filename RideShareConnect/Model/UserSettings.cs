using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
    public class UserSettings
    {
        [Key]
        public int SettingsId { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; } // Required, non-nullable
        
        public bool EmailNotifications { get; set; } = false; // Default value
        public bool SMSNotifications { get; set; } = false; // Default value
        public bool PushNotifications { get; set; } = false; // Default value
        [Required]
        public string Language { get; set; } // Required, non-nullable
        [Required]
        public string Currency { get; set; } // Required, non-nullable
        [Required]
        public string TimeZone { get; set; } // Required, non-nullable
    }
}