// ✅ All using statements at the top
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideShareConnect.Model
{
    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int UserId { get; set; }
    }
}
