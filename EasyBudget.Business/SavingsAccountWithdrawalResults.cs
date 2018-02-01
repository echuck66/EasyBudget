﻿using System;
using EasyBudget.Models;

namespace EasyBudget.Business
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
