using System;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class BankTransferResults : UnitOfWorkResults<BankAccountFundsTransfer>
    {
        
        public decimal sourceAccountBeginningBalance { get; set; }

        public decimal sourceAccountEndingBalance { get; set; }

        public decimal destinationAccountBeginningBalance { get; set; }

        public decimal destinationAccountEndingBalance { get; set; }

        public BankAccount sourceAccount { get; set; }

        public Guid sourceAccountId { get; set; }

        public BankAccountType sourceAccountType { get; set; }

        public BankAccount destinationAccount { get; set; }

        public Guid destinationAccountId { get; set; }

        public BankAccountType destinationAccountType { get; set; }

        public decimal transactionAmount { get; set; }

        public BankTransferResults()
        {
        }
    }
}
