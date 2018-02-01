using System;
using System.Collections.Generic;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class CheckingAccountsResults : UnitOfWorkResults<ICollection<CheckingAccount>>
    {
        public CheckingAccountsResults()
        {
        }
    }
}
