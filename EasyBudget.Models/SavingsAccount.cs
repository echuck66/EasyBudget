using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models
{
    [SQLite.Table("SavingsAccount")]
    public class SavingsAccount : BankAccount
    {
        public string routingNumber { get; set; }

        public string accountNumber { get; set; }

        [MaxLength(250)]
        public string bankName { get; set; }

        [MaxLength(75), Unique]
        public string accountNickname { get; set; }

        public decimal currentBalance { get; set; }

        [NotMapped]
        public ICollection<SavingsDeposit> deposits { get; set; }

        [NotMapped]
        public ICollection<SavingsWithdrawal> withdrawals { get; set; }

        public SavingsAccount()
        {
        }
    }
}
