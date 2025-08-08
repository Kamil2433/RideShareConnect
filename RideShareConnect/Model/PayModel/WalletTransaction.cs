using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RideShareConnect.Models.PayModel; // ✅ Make sure this line exists!

namespace RideShareConnect.Models
{
    public class WalletTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int WalletId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

       public DateTime? TransactionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Type { get; set; } = "Credit"; // Credit or Debit

        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }
    }
}
