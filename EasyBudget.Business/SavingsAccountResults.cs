using System;
namespace EasyBudget.Business
{
    public class SavingsAccountTransactionUnitOfWorkResults
    {
        public Guid SavingsAccountId { get; set; }

        public bool Successful { get; set; }

        public string Message { get; set; }

        public Exception WorkException { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal EndingAccountBalance { get; set; }

        public SavingsAccountTransactionUnitOfWorkResults()
        {
        }
    }
}
