using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models
{
    [SQLite.Table("SavingsDeposit")]
    public class SavingsDeposit : BaseObject
    {
        public Guid savingsAccountId { get; set; }

        public virtual SavingsAccount savingsAccount { get; set; }

        public DateTime transactionDate { get; set; }

        public decimal transactionAmount { get; set; }

        [MaxLength(250)]
        public string description { get; set; }

        [MaxLength(250)]
        public string notation { get; set; }

        public bool recurring { get; set; }

        public Guid? budgetIncomeId { get; set; }

        public virtual IncomeItem budgetIncome { get; set; }

        public bool reconciled { get; set; }

        public SavingsDeposit()
        {
        }
    }
}
