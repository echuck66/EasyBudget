using System;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
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
