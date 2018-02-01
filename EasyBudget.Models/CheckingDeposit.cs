using System;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models
{
    [SQLite.Table("CheckingDeposit")]
    public class CheckingDeposit : BaseObject
    {
        public Guid checkingAccountId { get; set; }

        public virtual CheckingAccount checkingAccount { get; set; }

        public DateTime transactionDate { get; set; }

        public decimal transactionAmount { get; set; }

        [MaxLength(250)]
        public string description { get; set; }

        [MaxLength(250)]
        public string notation { get; set; }

        public Guid? budgetIncomeId { get; set; }

        public virtual IncomeItem budgetIncome { get; set; }

        public bool reconciled { get; set; }

        public CheckingDeposit()
        {
        }
    }
}
