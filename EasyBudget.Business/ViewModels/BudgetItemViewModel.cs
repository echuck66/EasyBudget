//
//  Copyright 2018  CrawfordNET Solutions, LLC
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetItemViewModel : BaseViewModel
    {
        BudgetItem source { get; set; }

        public int CategoryId { get; set; }

        public int BudgetItemId { get; set; }

        public string CategoryName { get; set; }

        public BudgetItemType ItemType { get; set; }

        public decimal BudgetedAmount { get; set; }

        public string ItemDescription { get; set; }

        public string ItemNotation { get; set; }

        public bool IsRecurring { get; set; }

        public Frequency ItemFrequency { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool IsNew { get; set; }

        internal BudgetItemViewModel(string dbFilePath)
            : base(dbFilePath)
        {

        }

        internal async Task PopulateVMAsync(int id, BudgetItemType itemType)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                switch (itemType)
                {
                    case BudgetItemType.Expense:
                        var _resultsExpenseItem = await uow.GetExpenseItemAsync(id);
                        if (_resultsExpenseItem.Successful)
                        {
                            this.source = _resultsExpenseItem.Results;
                        }
                        else
                        {
                            if (_resultsExpenseItem.WorkException != null)
                            {
                                WriteErrorCondition(_resultsExpenseItem.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsExpenseItem.Message))
                            {
                                WriteErrorCondition(_resultsExpenseItem.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred");
                            }
                        }
                        break;
                    case BudgetItemType.Income:
                        var _resultsIncomeItem = await uow.GetIncomeItemAsync(id);
                        if (_resultsIncomeItem.Successful)
                        {
                            this.source = _resultsIncomeItem.Results;
                        }
                        else
                        {
                            if (_resultsIncomeItem.WorkException != null)
                            {
                                WriteErrorCondition(_resultsIncomeItem.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsIncomeItem.Message))
                            {
                                WriteErrorCondition(_resultsIncomeItem.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred");
                            }
                        }
                        break;
                }

                if (this.source != null)
                {
                    var _resultsCategory = await uow.GetBudgetCategoryAsync(this.source.budgetCategoryId);
                    if (_resultsCategory.Successful)
                    {
                        this.source.budgetCategory = _resultsCategory.Results;
                        this.CategoryName = this.source.budgetCategory.categoryName;
                        this.BudgetedAmount = this.source.BudgetedAmount;
                        this.BudgetItemId = this.source.id;
                        this.EndDate = this.source.EndDate;
                        this.StartDate = this.source.StartDate;
                        this.IsRecurring = this.source.recurring;
                        this.ItemDescription = this.source.description;
                        this.ItemFrequency = this.source.frequency;
                        this.ItemNotation = this.source.notation;
                        this.ItemType = this.source.ItemType;
                        this.CanEdit = this.source.CanEdit;
                        this.CanDelete = this.source.CanDelete;
                        this.IsNew = this.source.IsNew;
                    }
                    else
                    {
                        if (_resultsCategory.WorkException != null)
                        {
                            WriteErrorCondition(_resultsCategory.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsCategory.Message))
                        {
                            WriteErrorCondition(_resultsCategory.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
                }
            }
        }
    }

}
