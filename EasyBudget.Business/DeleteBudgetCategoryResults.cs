using System;
using EasyBudget.Models;

namespace EasyBudget.Business
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
