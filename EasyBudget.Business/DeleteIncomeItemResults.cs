using System;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class DeleteIncomeItemResults : UnitOfWorkResults<bool>
    {
        public Guid IncomeItemId { get; set; }

        public IncomeItem IncomeItem { get; set; }

        public Guid BudgetCategoryId { get; set; }

        public BudgetCategory BudgetCategory { get; set; }

        public decimal PreviousBudgetedAmount { get; set; }

        public decimal NewBudgetedAmount { get; set; }

        public decimal PreviousBudgetCategoryAmount { get; set; }

        public decimal NewBudgetCategoryAmount { get; set; }

        public DeleteIncomeItemResults()
        {
        }
    }
}
