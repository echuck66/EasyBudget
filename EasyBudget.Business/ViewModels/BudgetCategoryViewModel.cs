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
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Business.ViewModels
{

    public class BudgetCategoryViewModel : BaseViewModel
    {
        BudgetCategory Category { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool IsSystemCategory { get; set; }

        public bool IsUserSelected { get; set; }

        public AppIcon CategoryIcon { get; set; }

        public BudgetCategoryType CategoryType { get; set; }

        public ICollection<BudgetItemViewModel> BudgetItemVMs { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool IsNew { get; set; }

        public BudgetCategoryViewModel(string dbFilePath)
            : base(dbFilePath)
        {
            this.BudgetItemVMs = new List<BudgetItemViewModel>();
        }

        internal async Task PopulateVMAsync(BudgetCategory category)
        {

            this.Category = category;
            this.CategoryId = category.id;
            this.Name = category.categoryName;
            this.Description = category.description;
            this.Amount = category.budgetAmount;
            this.IsSystemCategory = category.systemCategory;
            this.IsUserSelected = category.userSelected;
            this.CategoryIcon = category.categoryIcon;
            this.CategoryType = category.categoryType;
            this.IsNew = category.IsNew;
            this.CanEdit = category.CanEdit;
            this.CanDelete = category.CanDelete;

            using (UnitOfWork uow = new UnitOfWork(dbFilePath))
            {
                switch (category.categoryType)
                {
                    case BudgetCategoryType.Expense:
                        var _resultsExpenseItems = await uow.GetCategoryExpenseItemsAsync(category);
                        if (_resultsExpenseItems.Successful)
                        {
                            foreach (var item in _resultsExpenseItems.Results)
                            {
                                item.ItemType = BudgetItemType.Expense;
                                item.budgetCategory = category;
                                var vm = new BudgetItemViewModel(this.dbFilePath);
                                await vm.PopulateVMAsync(item.id, item.ItemType);
                                this.BudgetItemVMs.Add(vm);
                            }
                        }
                        else
                        {
                            if (_resultsExpenseItems.WorkException != null)
                            {
                                WriteErrorCondition(_resultsExpenseItems.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsExpenseItems.Message))
                            {
                                WriteErrorCondition(_resultsExpenseItems.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred loading Expense Items");
                            }
                        }
                        break;
                    case BudgetCategoryType.Income:
                        var _resultsIncomeItems = await uow.GetCategoryIncomeItemsAsync(category);
                        if (_resultsIncomeItems.Successful)
                        {
                            foreach (var item in _resultsIncomeItems.Results)
                            {
                                item.ItemType = BudgetItemType.Income;
                                item.budgetCategory = category;
                                var vm = new BudgetItemViewModel(this.dbFilePath);
                                await vm.PopulateVMAsync(item.id, item.ItemType);
                                this.BudgetItemVMs.Add(vm);
                            }
                        }
                        else
                        {
                            if (_resultsIncomeItems.WorkException != null)
                            {
                                WriteErrorCondition(_resultsIncomeItems.WorkException);
                            }
                            else if (!string.IsNullOrEmpty(_resultsIncomeItems.Message))
                            {
                                WriteErrorCondition(_resultsIncomeItems.Message);
                            }
                            else
                            {
                                WriteErrorCondition("An unknown error has occurred loading Income Items");
                            }
                        }
                        break;
                }
            }

        }


        internal async Task LoadBudgetCategoryDetails(int categoryId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetBudgetCategoryAsync(categoryId);
                if (_results.Successful)
                {
                    await PopulateVMAsync(_results.Results);
                }
                else
                {
                    if (_results.WorkException != null)
                    {
                        WriteErrorCondition(_results.WorkException);
                    }
                    else if (!string.IsNullOrEmpty(_results.Message))
                    {
                        WriteErrorCondition(_results.Message);
                    }
                    else
                    {
                        WriteErrorCondition("An unknown error has occurred loading the Budget Category");
                    }
                }
            }
        }

        internal void CreateBudgetCategory()
        {
            this.Category = new BudgetCategory();
            this.Category.IsNew = true;
            this.IsNew = true;

        }

        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;
            this.Category.categoryName = this.Name;
            this.Category.description = this.Description;
            this.Category.categoryType = this.CategoryType;
            this.Category.budgetAmount = this.Amount;
            this.Category.systemCategory = this.IsSystemCategory;
            this.Category.categoryIcon = this.CategoryIcon;
            this.Category.userSelected = this.IsUserSelected;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.IsNew)
                {

                    var _resultsAddCategory = await uow.AddBudgetCategoryAsync(this.Category);
                    if (_resultsAddCategory.Successful)
                    {
                        _saveOk = true;
                        this.CategoryId = _resultsAddCategory.Results.id;
                        this.IsNew = false;
                    }
                    else
                    {
                        _saveOk = false;
                        if (_resultsAddCategory.WorkException != null)
                        {
                            WriteErrorCondition(_resultsAddCategory.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsAddCategory.Message))
                        {
                            WriteErrorCondition(_resultsAddCategory.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
                }
                else
                {
                    var _resultsUpdateCategory = await uow.UpdateBudgetCategoryAsync(this.Category);
                    if (_resultsUpdateCategory.Successful)
                    {
                        _saveOk = true;
                        this.Category.id = _resultsUpdateCategory.Results.id;
                        this.IsNew = false;
                    }
                    else
                    {
                        _saveOk = false;
                        if (_resultsUpdateCategory.WorkException != null)
                        {
                            WriteErrorCondition(_resultsUpdateCategory.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsUpdateCategory.Message))
                        {
                            WriteErrorCondition(_resultsUpdateCategory.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }

                    }
                }
                if (_saveOk)
                {

                    //foreach (IncomeItem itm in this.IncomeItems)
                    //{
                    //if (itm.IsNew)
                    //{
                    //    var _resultsAddItem = await uow.AddIncomeItemAsync(itm);
                    //    if (_resultsAddItem.Successful)
                    //    {
                    //        _saveOk = _saveOk && true;
                    //    }
                    //    else
                    //    {
                    //        _saveOk = false;
                    //        if (_resultsAddItem.WorkException != null)
                    //        {
                    //            WriteErrorCondition(_resultsAddItem.WorkException);
                    //        }
                    //        else if (!string.IsNullOrEmpty(_resultsAddItem.Message))
                    //        {
                    //            WriteErrorCondition(_resultsAddItem.Message);
                    //        }
                    //        else
                    //        {
                    //            WriteErrorCondition("An unknown error has occurred");
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    var _resultsUpdateItem = await uow.UpdateIncomeItemAsync(itm);
                    //    if (_resultsUpdateItem.Successful)
                    //    {
                    //        _saveOk = _saveOk && true;
                    //    }
                    //    else
                    //    {
                    //        _saveOk = false;
                    //        if (_resultsUpdateItem.WorkException != null)
                    //        {
                    //            WriteErrorCondition(_resultsUpdateItem.WorkException);
                    //        }
                    //        else if (!string.IsNullOrEmpty(_resultsUpdateItem.Message))
                    //        {
                    //            WriteErrorCondition(_resultsUpdateItem.Message);
                    //        }
                    //        else
                    //        {
                    //            WriteErrorCondition("An unknown error has occurred");
                    //        }
                    //    }
                    //}
                    //}
                    //foreach (ExpenseItem itm in this.ExpenseItems)
                    //{
                    //    if (itm.IsNew)
                    //    {
                    //        var _resultsAddItem = await uow.AddExpenseItemAsync(itm);
                    //        if (_resultsAddItem.Successful)
                    //        {
                    //            _saveOk = _saveOk && true;
                    //        }
                    //        else
                    //        {
                    //            _saveOk = false;
                    //            if (_resultsAddItem.WorkException != null)
                    //            {
                    //                WriteErrorCondition(_resultsAddItem.WorkException);
                    //            }
                    //            else if (!string.IsNullOrEmpty(_resultsAddItem.Message))
                    //            {
                    //                WriteErrorCondition(_resultsAddItem.Message);
                    //            }
                    //            else
                    //            {
                    //                WriteErrorCondition("An unknown error has occurred");
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var _resultsUpdateItem = await uow.UpdateExpenseItemAsync(itm);
                    //        if (_resultsUpdateItem.Successful)
                    //        {
                    //            _saveOk = _saveOk && true;
                    //        }
                    //        else
                    //        {
                    //            _saveOk = false;
                    //            if (_resultsUpdateItem.WorkException != null)
                    //            {
                    //                WriteErrorCondition(_resultsUpdateItem.WorkException);
                    //            }
                    //            else if (!string.IsNullOrEmpty(_resultsUpdateItem.Message))
                    //            {
                    //                WriteErrorCondition(_resultsUpdateItem.Message);
                    //            }
                    //            else
                    //            {
                    //                WriteErrorCondition("An unknown error has occurred");
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }

        public ExpenseItem AddExpenseItem()
        {
            ExpenseItem item = new ExpenseItem();
            item.budgetCategoryId = this.Category.id;
            item.IsNew = true;

            //this.ExpenseItems.Add(item);

            return item;
        }

        public IncomeItem AddIncomeItem()
        {
            IncomeItem item = new IncomeItem();
            item.budgetCategoryId = this.Category.id;
            item.IsNew = true;
            //this.IncomeItems.Add(item);

            return item;
        }
    }

    public class BudgetCategoryViewModelComparer : IEqualityComparer<BudgetCategoryViewModel>
    {
        public bool Equals(BudgetCategoryViewModel x, BudgetCategoryViewModel y)
        {
            return x.CategoryId == y.CategoryId;
        }

        public int GetHashCode(BudgetCategoryViewModel obj)
        {
            int hashCode = obj.CategoryId.GetHashCode() + obj.Name.GetHashCode() + obj.Description.GetHashCode();
            return hashCode;
        }
    }
}
