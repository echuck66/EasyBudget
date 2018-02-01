using System;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class SavingsAccountDepositResults : UnitOfWorkResults<SavingsDeposit>
    {
        public Guid AccountId { get; set; }

        public SavingsAccount Account { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal EndingAccountBalance { get; set; }

        public SavingsAccountDepositResults()
        {
        }
    }
}
