using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models.DataModels
{
    [SQLite.Table("SavingsWithdrawal")]
    public class SavingsWithdrawal : BaseObject
    {
        public Guid savingsAccountId { get; set; }

        public virtual SavingsAccount savingsAccount { get; set; }

        public DateTime transactionDate { get; set; }

        public decimal transactionAmount { get; set; }

        [MaxLength(250)]
        public string description { get; set; }

        [MaxLength(250)]
        public string notation { get; set; }

        public Guid? budgetExpenseId { get; set; }

        public virtual ExpenseItem budgetExpense { get; set; }

        public bool reconciled { get; set; }

        public SavingsWithdrawal()
        {
        }
    }
}
