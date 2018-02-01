using System;
using SQLite;

namespace EasyBudget.Models.DataModels
{
    [SQLite.Table("BudgetCategory")]
    public class BudgetCategory : BaseObject
    {
        public BudgetCategory()
        {
        }

        [MaxLength(250), Unique]
        public string categoryName { get; set; }

        public string description { get; set; }

        public decimal budgetAmount { get; set; }

        public bool systemCategory { get; set; }

        public bool userSelected { get; set; }

        public AppIcon categoryIcon { get; set; }

        public BudgetCategoryType categoryType { get; set; }
    }
}
