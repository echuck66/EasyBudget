using System;
using System.Collections.Generic;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class ExpenseItemsResults : UnitOfWorkResults<ICollection<ExpenseItem>>
    {
        public ExpenseItemsResults()
        {
        }
    }
}
