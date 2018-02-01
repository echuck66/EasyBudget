using System;
using System.Collections.Generic;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class ExpenseItemsResults : UnitOfWorkResults<ICollection<ExpenseItem>>
    {
        public ExpenseItemsResults()
        {
        }
    }
}
