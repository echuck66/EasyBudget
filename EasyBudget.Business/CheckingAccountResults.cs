using System;
namespace EasyBudget.Business
{
    public class CheckingAccountTransactionUnitOfWorkResults
    {
        public Guid CheckingAccountId { get; set; }

        public bool Successful { get; set; }

        public string Message { get; set; }

        public Exception WorkException { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal EndingAccountBalance { get; set; }

        public CheckingAccountTransactionUnitOfWorkResults()
        {
            
        }
    }
}
