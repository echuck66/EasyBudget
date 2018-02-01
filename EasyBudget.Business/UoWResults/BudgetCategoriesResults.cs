using System;
using System.Collections.Generic;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class BudgetCategoriesResults : UnitOfWorkResults<ICollection<BudgetCategory>>
    {
        public BudgetCategoriesResults()
        {
        }
    }
}
