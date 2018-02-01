﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EasyBudget.Models.DataModels
{
    [SQLite.Table("ExpenseItem")]
    public class ExpenseItem : BaseObject
    {
        public decimal budgetedAmount { get; set; }

        public Guid budgetCategoryId { get; set; }

        public virtual BudgetCategory budgetCategory { get; set; }

        [MaxLength(250)]
        public string description { get; set; }

        [MaxLength(250)]
        public string notation { get; set; }

        public bool recurring { get; set; }

        public ExpenseItem()
        {
        }
    }
}