using System;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class FundsTransferResults : UnitOfWorkResults<BankAccountFundsTransfer>
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

        public FundsTransferResults()
        {
        }
    }
}
