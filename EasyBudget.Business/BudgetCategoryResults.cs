using System;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class BudgetCategoryResults : UnitOfWorkResults<BudgetCategory>
    {
        public Guid CategoryId { get; set; }

        public BudgetCategory Category { get; set; }

        public BudgetCategoryResults()
        {
        }
    }
}
