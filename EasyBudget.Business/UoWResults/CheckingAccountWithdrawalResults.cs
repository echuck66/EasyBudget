using System;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class CheckingAccountWithdrawalResults : UnitOfWorkResults<CheckingWithdrawal>
    {
        public Guid AccountId { get; set; }

        public CheckingAccount Account { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal EndingAccountBalance { get; set; }

        public CheckingAccountWithdrawalResults()
        {
        }
    }
}
