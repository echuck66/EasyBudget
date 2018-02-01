using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models
{
    [SQLite.Table("CheckingAccount")]
    public class CheckingAccount : BankAccount
    {
        public string routingNumber { get; set; }

        public string accountNumber { get; set; }

        [MaxLength(250)]
        public string bankName { get; set; }

        [MaxLength(75), Unique]
        public string accountNickname { get; set; }

        public decimal currentBalance { get; set; }

        [NotMapped]
        public ICollection<CheckingDeposit> deposits { get; set; }

        [NotMapped]
        public ICollection<CheckingWithdrawal> withdrawals { get; set; }

        public CheckingAccount()
        {
        }
    }
}
