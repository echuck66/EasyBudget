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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using EasyBudget.Models;
using EasyBudget.Models.DataModels;

// TODO Convert VMs to only expose VM and ICollection<VM> object types instead of the actual
//      underlying data models.
//      e.g. 
//      BudgetCategoriesVM should expose 
//          ICollection<BudgetCategoryVM> 
//              NOT
//          ICollection<BudgetCategory>
//
// TODO Change iOS project's ViewControllers to reflect this change
//
namespace EasyBudget.Business
{
    public sealed class EasyBudgetDataService
    {
        string dbFilePath;

        public EasyBudgetDataService(string dbFilePath)
        {
            this.dbFilePath = dbFilePath;
        }

        private async Task EnsureSystemItemsExistAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.EnsureSystemDataItemsAsync();
                if (_results.Successful)
                {
                    var _categoryCount = _results.BudgetCategoriesCount;
                }
                else
                {
                    var _categoriesExist = _results.BudgetCategoriesExist;
                }
            }
        }

        public async Task<BankAccountsVM> GetBankAccountsViewModelAsync()
        {
            BankAccountsVM vm = new BankAccountsVM(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadBankAccountsAsync();
            return vm;
        }

        public async Task<BudgetCategoriesVM> GetBudgetCategoriesViewModelAsync()
        {
            BudgetCategoriesVM vm = new BudgetCategoriesVM(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetCategoriesAsync();

            return vm;
        }

        public async Task<BudgetCategoryVM> GetBudgetCategoryVM(int categoryId)
        {
            BudgetCategoryVM vm = new BudgetCategoryVM(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetCategoryDetails(categoryId);

            return vm;
        }

        public async Task<BudgetItemsVM> GetBudgetItemsVM(int categoryId)
        {
            BudgetItemsVM vm = new BudgetItemsVM(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetItemsAsync(categoryId);

            return vm;
        }

        public async Task<CheckingAccountVM> GetCheckingAccountVMAsync(int accountId)
        {
            CheckingAccountVM vm = new CheckingAccountVM(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadCheckingAccountDetailsAsync(accountId);

            return vm;
        }

        public async Task<SavingsAccountVM> GetSavingsAccountVMAsync(int accountId)
        {
            SavingsAccountVM vm = new SavingsAccountVM(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadSavingsAccountDetailsAsync(accountId);

            return vm;
        }

        public async Task<IncomeItemVM> GetIncomeItemVMAsync(int itemId)
        {
            IncomeItemVM vm = new IncomeItemVM(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadIncomeItemAsync(itemId);

            return vm;
        }

        public async Task<ExpenseItemVM> GetExpenseItemVMAsync(int itemId)
        {
            ExpenseItemVM vm = new ExpenseItemVM(this.dbFilePath);
            await this.EnsureSystemItemsExistAsync();
            await vm.LoadExpenseItemAsync(itemId);

            return vm;
        }
    
        //public async Task<bool> DeleteBudgetCategoryAsync(BudgetCategory category)
        //{
        //    bool _success = false;  
        //    using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
        //    {
        //        var _resultsIncomeItems = await uow.GetCategoryIncomeItemsAsync(category);
        //        if (_resultsIncomeItems.Successful)
        //        {
        //            foreach(IncomeItem itm in _resultsIncomeItems.Results)
        //            {
        //                var _resDel = await uow.DeleteIncomeItemAsync(itm);
        //            }
        //        }
        //        var _resultsExpenseItems = await uow.GetCategoryExpenseItemsAsync(category);
        //        if (_resultsExpenseItems.Successful)
        //        {
        //            foreach(ExpenseItem itm in _resultsExpenseItems.Results)
        //            {
        //                var _resDel = await uow.DeleteExpenseItemAsync(itm);
        //            }
        //        }
        //        var _resultsDeleteCategory = await uow.DeleteBudgetCategoryAsync(category);
        //        _success = _resultsDeleteCategory.Successful;
        //    }
        //    return _success;
        //}
    
        public BudgetCategoryVM CreateBudgetCategoryVM()
        {
            var vm = new BudgetCategoryVM(this.dbFilePath);
            vm.CreateBudgetCategory();

            return vm;
        }

        public CheckingAccountVM CreateCheckingAccountVM()
        {
            var vm = new CheckingAccountVM(this.dbFilePath);
            vm.CreateCheckingAccount();

            return vm;
        }

        public SavingsAccountVM CreateSavingsAccountVM()
        {
            var vm = new SavingsAccountVM(this.dbFilePath);
            vm.CreateSavingsAccount();

            return vm;
        }
    }

    public abstract class BaseViewModel
    {
        internal string dbFilePath;

        public string ErrorCondition { get; set; }
        
        public BaseViewModel(string dbFilePath)
        {
            this.dbFilePath = dbFilePath;
        }

        internal void WriteErrorCondition(string error)
        {
            if (string.IsNullOrEmpty(this.ErrorCondition))
                this.ErrorCondition = error;
            else
            {
                StringBuilder sb = new StringBuilder(this.ErrorCondition);
                sb.AppendLine(error);
                this.ErrorCondition = sb.ToString();
            }
        }

        internal void WriteErrorCondition(Exception ex)
        {
            if (string.IsNullOrEmpty(this.ErrorCondition))
                this.ErrorCondition = ex.Message;
            else
            {
                StringBuilder sb = new StringBuilder(this.ErrorCondition);
                sb.AppendLine(ex.Message);
                this.ErrorCondition = sb.ToString();
            }
            if (ex.InnerException != null)
            {
                WriteErrorCondition(ex.InnerException);
            }
        }
    }

    public class BankAccountsVM : BaseViewModel
    {

        public ICollection<BankAccount> BankAccounts { get; set; }

        public BankAccountsVM(string dbFilePath)
            : base(dbFilePath)
        {
            BankAccounts = new List<BankAccount>();
        }

        internal async Task LoadBankAccountsAsync()
        {
            await LoadCheckingAccountsAsync();
            await LoadSavingsAccountsAsync();
        }

        private async Task LoadCheckingAccountsAsync()
        {

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllCheckingAccountsAsync();
                if (_results.Successful)
                {
                    foreach (var account in _results.Results)
                    {
                        this.BankAccounts.Add(account);
                    }
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
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }
            }
        }

        private async Task LoadSavingsAccountsAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllSavingsAccountsAsync();
                if (_results.Successful)
                {
                    foreach (var account in _results.Results)
                    {
                        this.BankAccounts.Add(account);
                    }
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
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }
            }
        }

    }

    public class BankAccountVM : BaseViewModel
    {
        BankAccount Account { get; set; }

        public string BankName { get; set; }

        public BankAccountType AccountType { get; set; }

        public decimal CurrentBalance { get; set; }

        public string RoutingNumber { get; set; }

        public string AccountNumber { get; set; }

        public string Nickname { get; set; }


        public BankAccountVM(string dbFilePath)
            :base(dbFilePath)
        {
            
        }
    }

    public class BudgetItemsVM : BaseViewModel
    {

        List<BudgetItem> BudgetItems { get; set; }

        ICollection<BudgetItemVM> BudgetItemVMs { get; set; }

        public BudgetItemsVM(string dbFilePath)
            : base(dbFilePath)
        {
            BudgetItems = new List<BudgetItem>();
            BudgetItemVMs = new List<BudgetItemVM>();
        }

        internal async Task LoadBudgetItemsAsync(int categoryId)
        {
            await LoadIncomeItemsAsync(categoryId);
            await LoadExpenseItemsAsync(categoryId);
        }

        private async Task LoadIncomeItemsAsync(int categoryId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _resultsCategory = await uow.GetBudgetCategoryAsync(categoryId);
                if (_resultsCategory.Successful)
                {
                    BudgetCategory category = _resultsCategory.Results;
                    var _resultsIncomeItems = await uow.GetCategoryIncomeItemsAsync(category);
                    if (_resultsIncomeItems.Successful)
                    {
                        foreach (var item in _resultsIncomeItems.Results)
                        {
                            item.ItemType = BudgetItemType.Income;
                            this.BudgetItems.Add(item);
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
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
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

        private async Task LoadExpenseItemsAsync(int categoryId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _resultsCategory = await uow.GetBudgetCategoryAsync(categoryId);
                if (_resultsCategory.Successful)
                {
                    BudgetCategory category = _resultsCategory.Results;
                    var _resultsIncomeItems = await uow.GetCategoryExpenseItemsAsync(category);
                    if (_resultsIncomeItems.Successful)
                    {
                        foreach (var item in _resultsIncomeItems.Results)
                        {
                            item.ItemType = BudgetItemType.Expense;
                            this.BudgetItems.Add(item);
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
                            WriteErrorCondition("An unknown error has occurred loading Expense Items");
                        }
                    }
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

    public class BudgetItemVM : BaseViewModel
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

        public BudgetItemVM(string dbFilePath)
            :base(dbFilePath)
        {
            
        }

        public BudgetItemVM(string dbFilePath, BudgetItem budgetItem)
            :base(dbFilePath)
        {
            PopulateVM(budgetItem);
        }

        internal void PopulateVM(BudgetItem budgetItem)
        {
            this.source = budgetItem;
            this.CategoryId = budgetItem.budgetCategoryId;
            this.BudgetItemId = budgetItem.id;
            this.CategoryName = budgetItem.budgetCategory.categoryName;
            this.ItemType = budgetItem.ItemType;
            this.BudgetedAmount = budgetItem.BudgetedAmount;
            this.ItemDescription = budgetItem.description;
            this.ItemNotation = budgetItem.notation;
            this.IsRecurring = budgetItem.recurring;
            this.ItemFrequency = budgetItem.frequency;
            this.StartDate = budgetItem.StartDate;
            this.EndDate = budgetItem.EndDate;

        }


    }
    public class BudgetCategoriesVM : BaseViewModel
    {

        public ICollection<BudgetCategory> BudgetCategories { get; set; }

        public ICollection<BudgetCategoryVM> BudgetCategoryVMs { get; set; }

        public BudgetCategoriesVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        internal async Task LoadBudgetCategoriesAsync()
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                if (_results.Successful)
                {
                    this.BudgetCategories = _results.Results;
                    foreach (var category in _results.Results)
                    {
                        this.BudgetCategoryVMs.Add(new BudgetCategoryVM(this.dbFilePath, category));
                    }
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
                        WriteErrorCondition("An unknown error has occurred");
                    }
                }
            }
        }

    }

    public class BudgetCategoryVM : BaseViewModel
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

        public ICollection<BudgetItemVM> BudgetItems { get; set; }

        public bool IsNew { get; set; }

        public BudgetCategoryVM(string dbFilePath)
            : base(dbFilePath)
        {
            //this.IncomeItems = new List<IncomeItem>();
            //this.ExpenseItems = new List<ExpenseItem>();
        }

        public BudgetCategoryVM(string dbFilePath, BudgetCategory category)
            :base(dbFilePath)
        {
            PopulateVM(category);
        }

        private void PopulateVM(BudgetCategory category)
        {
            this.Category = category;
            this.CategoryId = this.Category.id;
            this.Name = this.Category.categoryName;
            this.Description = this.Category.description;
            this.Amount = this.Category.budgetAmount;
            this.IsSystemCategory = this.Category.systemCategory;
            this.IsUserSelected = this.Category.userSelected;
            this.CategoryIcon = this.Category.categoryIcon;
            this.CategoryType = this.Category.categoryType;
            this.IsNew = this.Category.IsNew;

        }

        internal async Task LoadBudgetCategoryDetails(int categoryId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetBudgetCategoryAsync(categoryId);
                if (_results.Successful)
                {
                    PopulateVM(_results.Results);

                    if (this.Category != null) 
                    {
                        switch(this.Category.categoryType)
                        {
                            case BudgetCategoryType.Expense:
                                var _resultsExpenseItems = await uow.GetCategoryExpenseItemsAsync(this.Category);
                                if (_resultsExpenseItems.Successful)
                                {
                                    foreach(var item in _resultsExpenseItems.Results)
                                    {
                                        item.ItemType = BudgetItemType.Expense;
                                        item.budgetCategory = this.Category;
                                        this.BudgetItems.Add(new BudgetItemVM(this.dbFilePath, item));
                                        //this.ExpenseItems.Add(item);
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
                                var _resultsIncomeItems = await uow.GetCategoryIncomeItemsAsync(this.Category);
                                if (_resultsIncomeItems.Successful)
                                {
                                    foreach(var item in _resultsIncomeItems.Results)
                                    {
                                        item.ItemType = BudgetItemType.Income;
                                        item.budgetCategory = this.Category;
                                        this.BudgetItems.Add(new BudgetItemVM(this.dbFilePath, item));
                                        //this.IncomeItems.Add(item);
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

        }

        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.Category.IsNew)
                {
                    var _resultsAddCategory = await uow.AddBudgetCategoryAsync(this.Category);
                    if (_resultsAddCategory.Successful)
                    {
                        _saveOk = true;

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

    public class CheckingAccountVM : BaseViewModel
    {
        CheckingAccount CheckingAccount { get; set; }

        public string RoutingNumber { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string AccountNickname { get; set; }

        public decimal CurrentBalance { get; set; }

        public DateTime LoadTransactionsFromDate { get; set; }

        public DateTime LoadTransactionsToDate { get; set; }

        public ICollection<CheckingWithdrawal> Withdrawals { get; set; }

        public ICollection<CheckingDeposit> Deposits { get; set; }

        public CheckingAccountVM(string dbFilePath)
            : base(dbFilePath)
        {
            this.Withdrawals = new List<CheckingWithdrawal>();
            this.Deposits = new List<CheckingDeposit>();
        }

        internal async Task LoadCheckingAccountDetailsAsync(int accountId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetCheckingAccountAsync(accountId);
                if (_results.Successful)
                {
                    this.CheckingAccount = _results.Results;
                    this.RoutingNumber = this.CheckingAccount?.routingNumber ?? string.Empty;
                    this.AccountNumber = this.CheckingAccount?.accountNumber ?? string.Empty;
                    this.BankName = this.CheckingAccount?.bankName ?? string.Empty;
                    this.AccountNickname = this.CheckingAccount?.accountNickname ?? string.Empty;
                    this.CurrentBalance = this.CheckingAccount?.currentBalance ?? 0;
                    if (this.CheckingAccount.withdrawals != null)
                    {
                        foreach(CheckingWithdrawal item in this.CheckingAccount.withdrawals)
                        {
                            this.Withdrawals.Add(item);
                        }
                    }
                    if (this.CheckingAccount.deposits != null)
                    {
                        foreach(CheckingDeposit item in this.CheckingAccount.deposits)
                        {
                            this.Deposits.Add(item);
                        }
                    }
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
                        WriteErrorCondition("An unknown error has occurred loading Checking Account");
                    }
                }
            }
        }
    
        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.CheckingAccount.IsNew)
                {
                    var _resultsAddAccont = await uow.AddCheckingAccountAsync(this.CheckingAccount);
                    if (_resultsAddAccont.Successful)
                    {
                        _saveOk = true;
                    }
                    else
                    {
                        _saveOk = false;
                        if (_resultsAddAccont.WorkException != null)
                        {
                            WriteErrorCondition(_resultsAddAccont.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsAddAccont.Message))
                        {
                            WriteErrorCondition(_resultsAddAccont.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
                }
                else
                {
                    var _resultsUpdateAccont = await uow.UpdateCheckingAccountAsync(this.CheckingAccount);
                    if (_resultsUpdateAccont.Successful)
                    {
                        _saveOk = true;
                    }
                    else
                    {
                        _saveOk = false;
                        if (_resultsUpdateAccont.WorkException != null)
                        {
                            WriteErrorCondition(_resultsUpdateAccont.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsUpdateAccont.Message))
                        {
                            WriteErrorCondition(_resultsUpdateAccont.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
                }
                if (_saveOk)
                {
                    foreach(CheckingDeposit deposit in this.Deposits)
                    {
                        if (deposit.IsNew)
                        {
                            var _resultsAddDeposit = await uow.AddCheckingDepositAsync(deposit);
                            if (_resultsAddDeposit.Successful)
                            {
                                _saveOk = _saveOk && true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsAddDeposit.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddDeposit.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddDeposit.Message))
                                {
                                    WriteErrorCondition(_resultsAddDeposit.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                        else
                        {
                            var _resultsUpdateDeposit = await uow.UpdateCheckingDepositAsync(deposit);
                            if (_resultsUpdateDeposit.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsUpdateDeposit.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsUpdateDeposit.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsUpdateDeposit.Message))
                                {
                                    WriteErrorCondition(_resultsUpdateDeposit.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                    }
                    foreach (CheckingWithdrawal withdrawal in this.Withdrawals)
                    {
                        if (withdrawal.IsNew)
                        {
                            var _resultsAddWithdrawal = await uow.AddCheckingWithdrawalAsync(withdrawal);
                            if (_resultsAddWithdrawal.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsAddWithdrawal.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddWithdrawal.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddWithdrawal.Message))
                                {
                                    WriteErrorCondition(_resultsAddWithdrawal.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                        else 
                        {
                            var _resultsUpdateWithdrawal = await uow.UpdateCheckingWithdrawalAsync(withdrawal);
                            if (_resultsUpdateWithdrawal.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsUpdateWithdrawal.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsUpdateWithdrawal.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsUpdateWithdrawal.Message))
                                {
                                    WriteErrorCondition(_resultsUpdateWithdrawal.Message);
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

        internal void CreateCheckingAccount()
        {
            this.CheckingAccount = new CheckingAccount();
            this.CheckingAccount.IsNew = true;
        }

        public CheckingDeposit AddDeposit()
        {
            CheckingDeposit deposit = new CheckingDeposit();
            deposit.checkingAccountId = this.CheckingAccount.id;
            deposit.IsNew = true;

            this.Deposits.Add(deposit);

            return deposit;
        }

        public CheckingWithdrawal AddWithdrawal()
        {
            CheckingWithdrawal withdrawal = new CheckingWithdrawal();
            withdrawal.checkingAccountId = this.CheckingAccount.id;
            withdrawal.IsNew = true;

            this.Withdrawals.Add(withdrawal);

            return withdrawal;
        }
    }

    public class SavingsAccountVM : BaseViewModel
    {
        SavingsAccount SavingsAccount { get; set; }

        public string RoutingNumber { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string AccountNickname { get; set; }

        public decimal CurrentBalance { get; set; }

        public ICollection<SavingsWithdrawal> Withdrawals { get; set; }

        public ICollection<SavingsDeposit> Deposits { get; set; }

        public SavingsAccountVM(string dbFilePath)
            : base(dbFilePath)
        {
            this.Withdrawals = new List<SavingsWithdrawal>();
            this.Deposits = new List<SavingsDeposit>();
        }

        internal async Task LoadSavingsAccountDetailsAsync(int accountId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetSavingsAccountAsync(accountId);
                if (_results.Successful)
                {
                    this.SavingsAccount = _results.Results;
                    this.RoutingNumber = this.SavingsAccount?.routingNumber ?? string.Empty;
                    this.AccountNumber = this.SavingsAccount?.accountNumber ?? string.Empty;
                    this.BankName = this.SavingsAccount?.bankName ?? string.Empty;
                    this.AccountNickname = this.SavingsAccount?.accountNickname ?? string.Empty;
                    this.CurrentBalance = this.SavingsAccount?.currentBalance ?? 0;
                    if (this.SavingsAccount.withdrawals != null)
                    {
                        foreach (SavingsWithdrawal item in this.SavingsAccount.withdrawals)
                        {
                            this.Withdrawals.Add(item);
                        }
                    }
                    if (this.SavingsAccount.deposits != null)
                    {
                        foreach (SavingsDeposit item in this.SavingsAccount.deposits)
                        {
                            this.Deposits.Add(item);
                        }
                    }
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
                        WriteErrorCondition("An unknown error has occurred loading Savings Account");
                    }
                }
            }
        }
    
        internal void CreateSavingsAccount()
        {
            this.SavingsAccount = new SavingsAccount();
            this.SavingsAccount.IsNew = true;
        }

        public async Task SaveChangesAsync()
        {
            bool _saveOk = true;

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                if (this.SavingsAccount.IsNew)
                {
                    var _resultsAddAccont = await uow.AddSavingsAccountAsync(this.SavingsAccount);
                    if (_resultsAddAccont.Successful)
                    {
                        _saveOk = true;
                    }
                    else
                    {
                        _saveOk = false;
                        if (_resultsAddAccont.WorkException != null)
                        {
                            WriteErrorCondition(_resultsAddAccont.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsAddAccont.Message))
                        {
                            WriteErrorCondition(_resultsAddAccont.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
                }
                else
                {
                    var _resultsUpdateAccont = await uow.UpdateSavingsAccountAsync(this.SavingsAccount);
                    if (_resultsUpdateAccont.Successful)
                    {
                        _saveOk = true;
                    }
                    else
                    {
                        _saveOk = false;
                        if (_resultsUpdateAccont.WorkException != null)
                        {
                            WriteErrorCondition(_resultsUpdateAccont.WorkException);
                        }
                        else if (!string.IsNullOrEmpty(_resultsUpdateAccont.Message))
                        {
                            WriteErrorCondition(_resultsUpdateAccont.Message);
                        }
                        else
                        {
                            WriteErrorCondition("An unknown error has occurred");
                        }
                    }
                }
                if (_saveOk)
                {
                    foreach (SavingsDeposit deposit in this.Deposits)
                    {
                        if (deposit.IsNew)
                        {
                            var _resultsAddDeposit = await uow.AddSavingsDepositAsync(deposit);
                            if (_resultsAddDeposit.Successful)
                            {
                                _saveOk = _saveOk && true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsAddDeposit.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddDeposit.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddDeposit.Message))
                                {
                                    WriteErrorCondition(_resultsAddDeposit.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                        else
                        {
                            var _resultsUpdateDeposit = await uow.UpdateSavingsDepositAsync(deposit);
                            if (_resultsUpdateDeposit.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsUpdateDeposit.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsUpdateDeposit.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsUpdateDeposit.Message))
                                {
                                    WriteErrorCondition(_resultsUpdateDeposit.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                    }
                    foreach (SavingsWithdrawal withdrawal in this.Withdrawals)
                    {
                        if (withdrawal.IsNew)
                        {
                            var _resultsAddWithdrawal = await uow.AddSavingsWithdrawalAsync(withdrawal);
                            if (_resultsAddWithdrawal.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsAddWithdrawal.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsAddWithdrawal.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsAddWithdrawal.Message))
                                {
                                    WriteErrorCondition(_resultsAddWithdrawal.Message);
                                }
                                else
                                {
                                    WriteErrorCondition("An unknown error has occurred");
                                }
                            }
                        }
                        else
                        {
                            var _resultsUpdateWithdrawal = await uow.UpdateSavingsWithdrawalAsync(withdrawal);
                            if (_resultsUpdateWithdrawal.Successful)
                            {
                                _saveOk = true;
                            }
                            else
                            {
                                _saveOk = false;
                                if (_resultsUpdateWithdrawal.WorkException != null)
                                {
                                    WriteErrorCondition(_resultsUpdateWithdrawal.WorkException);
                                }
                                else if (!string.IsNullOrEmpty(_resultsUpdateWithdrawal.Message))
                                {
                                    WriteErrorCondition(_resultsUpdateWithdrawal.Message);
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

        public SavingsDeposit AddDeposit()
        {
            var deposit = new SavingsDeposit();
            deposit.savingsAccountId = this.SavingsAccount.id;
            deposit.IsNew = true;

            this.Deposits.Add(deposit);

            return deposit;
        }

        public SavingsWithdrawal AddWithdrawal()
        {
            var withdrawal = new SavingsWithdrawal();
            withdrawal.savingsAccountId = this.SavingsAccount.id;
            withdrawal.IsNew = true;

            this.Withdrawals.Add(withdrawal);

            return withdrawal;
        }
    }

    public class ExpenseItemVM : BaseViewModel
    {
        
        ExpenseItem ExpenseItem;

        public decimal BudgetedAmount { get; set; }

        public string Description { get; set; }

        public string Notation { get; set; }

        public bool Recurring { get; set; }

        public ExpenseItemVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        internal async Task LoadExpenseItemAsync(int itemId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetExpenseItemAsync(itemId);
                if (_results.Successful)
                {
                    
                    this.ExpenseItem = _results.Results;
                    this.ExpenseItem.ItemType = BudgetItemType.Expense;
                    this.BudgetedAmount = this.ExpenseItem?.BudgetedAmount ?? 0;
                    this.Description = this.ExpenseItem?.description ?? string.Empty;
                    this.Notation = this.ExpenseItem?.notation ?? string.Empty;
                    this.Recurring = this.ExpenseItem?.recurring ?? false;

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
                        WriteErrorCondition("An unknown error has occurred loading Expense Item");
                    }
                }
            }
        }

        internal void CrateExpenseItem(int categoryId)
        {
            this.ExpenseItem = new ExpenseItem();
            this.ExpenseItem.budgetCategoryId = categoryId;
            this.ExpenseItem.IsNew = true;
        }
    }

    public class IncomeItemVM : BaseViewModel
    {
        IncomeItem IncomeItem;

        public decimal BudgetedAmount { get; set; }

        public string Description { get; set; }

        public string Notation { get; set; }

        public bool Recurring { get; set; }

        public IncomeItemVM(string dbFilePath)
            : base(dbFilePath)
        {

        }

        internal async Task LoadIncomeItemAsync(int itemId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetIncomeItemAsync(itemId);
                if (_results.Successful)
                {
                    this.IncomeItem = _results.Results;
                    this.IncomeItem.ItemType = BudgetItemType.Income;
                    this.BudgetedAmount = this.IncomeItem?.BudgetedAmount ?? 0;
                    this.Description = this.IncomeItem?.description ?? string.Empty;
                    this.Notation = this.IncomeItem?.notation ?? string.Empty;
                    this.Recurring = this.IncomeItem?.recurring ?? false;

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
                        WriteErrorCondition("An unknown error has occurred loading Income Item");
                    }
                }
            }
        }
    
        internal void CreateIncomeItem(int categoryId)
        {
            this.IncomeItem = new IncomeItem();
            this.IncomeItem.budgetCategoryId = categoryId;
            this.IncomeItem.IsNew = true;
        }
    }

}
