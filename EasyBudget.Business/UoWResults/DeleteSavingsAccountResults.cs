using System;
using System.Collections.Generic;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
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
