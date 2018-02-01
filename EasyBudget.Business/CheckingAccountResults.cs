using System;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class CheckingAccountResults : UnitOfWorkResults<CheckingAccount>
    {
        public Guid AccountId { get; set; }

        public CheckingAccount Account { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal EndingAccountBalance { get; set; }

        public CheckingAccountResults()
        {
            
        }
    }
}
