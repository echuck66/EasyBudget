using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models.DataModels
{
    [SQLite.Table("CheckingWithdrawal")]
    public class CheckingWithdrawal : BaseObject
    {
        public Guid checkingAccountId { get; set; }

        public virtual CheckingAccount checkingAccount { get; set; }

        public DateTime transactionDate { get; set; }

        public decimal transactionAmount { get; set; }

        public int checkNumber { get; set; }

        [MaxLength(250)]
        public string payToTheOrderOf { get; set; }

        [MaxLength(250)]
        public string memo { get; set; }

        public Guid? budgetExpenseId { get; set; }

        public virtual ExpenseItem budgetExpense { get; set; }

        public bool reconciled { get; set; }

        public bool isTaxDeductable { get; set; }

        public CheckingWithdrawal()
        {
        }
    }
}
