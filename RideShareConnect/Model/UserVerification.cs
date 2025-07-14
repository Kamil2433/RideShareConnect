using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Models
{
 public class UserVerification
{
    [Key]
    public int VerificationId { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    
    public string DocumentType { get; set; }
    public string DocumentNumber { get; set; }
    public string DocumentImageUrl { get; set; }
    public string Status { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DateTime VerifiedAt { get; set; }
    public string VerifiedBy { get; set; }
    public string Comments { get; set; }
}
}