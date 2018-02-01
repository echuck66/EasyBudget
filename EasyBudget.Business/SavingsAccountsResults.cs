using System;
using System.Collections.Generic;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class SavingsAccountsResults : UnitOfWorkResults<ICollection<SavingsAccount>>
    {
        public SavingsAccountsResults()
        {
        }
    }
}
