using System;
using System.Collections.Generic;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class DeleteSavingsAccountResults : UnitOfWorkResults<bool>
    {
        public Guid AccountId { get; set; }

        public SavingsAccount Account { get; set; }

        public DeleteSavingsAccountResults()
        {
        }
    }
}
