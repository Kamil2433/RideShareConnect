using System;
using System.Collections.Generic;

namespace RideShareFrontend.Models.DTOs
{
    public class WalletTransactionDto
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } // "Credit" or "Debit"
        public DateTime TransactionDate { get; set; }
    }

    public class WalletViewModel
    {
        public int WalletId { get; set; }
        public int UserId { get; set; }
        public decimal CurrentBalance { get; set; }
        public List<WalletTransactionDto> Transactions { get; set; } = new List<WalletTransactionDto>();
    }
}