using System;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class SavingsAccountWithdrawalResults : UnitOfWorkResults<SavingsWithdrawal>
    {
        public Guid AccountId { get; set; }

        public SavingsAccount Account { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal EndingAccountBalance { get; set; }

        public SavingsAccountWithdrawalResults()
        {
        }
    }
}
