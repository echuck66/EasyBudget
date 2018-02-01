﻿using System;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.UoWResults
{
    public class ExpenseItemResults : UnitOfWorkResults<ExpenseItem>
    {
        public Guid BudgetCategoryId { get; set; }

        public BudgetCategory BudgetCategory { get; set; }

        public decimal PreviousBudgetedAmount { get; set; }

        public decimal NewBudgetedAmount { get; set; }

        public decimal PreviousBudgetCategoryAmount { get; set; }

        public decimal NewBudgetCategoryAmount { get; set; }

        public ExpenseItemResults()
        {
        }
    }
}