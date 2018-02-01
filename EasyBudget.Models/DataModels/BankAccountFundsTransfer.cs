using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models.DataModels
{
    [SQLite.Table("BankAccountTransfer")]
    public class BankAccountFundsTransfer : BaseObject
    {
        [NotMapped]
        public BankAccount sourceAccount { get; set; }

        public Guid sourceAccountId { get; set; }

        public BankAccountType sourceAccountType { get; set; }

        [NotMapped]
        public BankAccount destinationAccount { get; set; }

        public Guid destinationAccountId { get; set; }

        public BankAccountType destinationAccountType { get; set; }

        public decimal transactionAmount { get; set; }

        public DateTime transactionDate { get; set; }

        public decimal sourceAccountBeginningBalance { get; set; }

        public decimal sourceAccountEndingBalance { get; set; }

        public decimal destinationAccountBeginningBalance { get; set; }

        public decimal destinationAccountEndingBalance { get; set; }

        public bool voided { get; set; }

        public BankAccountFundsTransfer()
        {
        }
    }
}
