using System;
using System.Collections.Generic;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class SavingsAccountsResults : UnitOfWorkResults<ICollection<SavingsAccount>>
    {
        public SavingsAccountsResults()
        {
        }
    }
}
