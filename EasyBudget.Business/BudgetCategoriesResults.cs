using System;
using System.Collections.Generic;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class BudgetCategoriesResults : UnitOfWorkResults<ICollection<BudgetCategory>>
    {
        public BudgetCategoriesResults()
        {
        }
    }
}
