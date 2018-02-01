using System;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class DeleteBudgetCategoryResults : UnitOfWorkResults<bool>
    {
        public Guid CategoryId { get; set; }

        public BudgetCategory Category { get; set; }

        public DeleteBudgetCategoryResults()
        {
        }
    }
}
