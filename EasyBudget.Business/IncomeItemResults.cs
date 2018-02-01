using System;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class BudgetItemUnitOfWorkResults
    {
        public Guid BudgetCategoryId { get; set; }

        public BudgetCategory BudgetCategory { get; set; }

        public decimal PreviousBudgetedAmount { get; set; }

        public decimal NewBudgetedAmount { get; set; }

        public decimal PreviousBudgetCategoryAmount { get; set; }

        public decimal NewBudgetCategoryAmount { get; set; }

        public bool Successful { get; set; }

        public string Message { get; set; }

        public Exception WorkException { get; set; }

        public BudgetItemUnitOfWorkResults()
        {
        }
    }
}
