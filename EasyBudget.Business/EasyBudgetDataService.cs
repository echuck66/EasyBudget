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
            //await this.EnsureSystemItemsExistAsync();
            await vm.LoadBankAccountsAsync();
            return vm;
        }

        public async Task<BudgetCategoriesVM> GetBudgetCategoriesViewModelAsync()
        {
            BudgetCategoriesVM vm = new BudgetCategoriesVM(this.dbFilePath);
            //await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetCategoriesAsync();

            return vm;
        }

        public async Task<BudgetCategoryVM> GetBudgetCategoryVM(int categoryId)
        {
            BudgetCategoryVM vm = new BudgetCategoryVM(this.dbFilePath);
            //await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetCategoryDetails(categoryId);

            return vm;
        }

        public async Task<BudgetItemsVM> GetBudgetItemsVM(int categoryId)
        {
            BudgetItemsVM vm = new BudgetItemsVM(this.dbFilePath);
            //await this.EnsureSystemItemsExistAsync();
            await vm.LoadBudgetItemsAsync(categoryId);

            return vm;
        }

        public async Task<CheckingAccountVM> GetCheckingAccountVMAsync(int accountId)
        {
            CheckingAccountVM vm = new CheckingAccountVM(this.dbFilePath);
            //await this.EnsureSystemItemsExistAsync();
            await vm.LoadCheckingAccountDetailsAsync(accountId);

            return vm;
        }

        public async Task<SavingsAccountVM> GetSavingsAccountVMAsync(int accountId)
        {
            SavingsAccountVM vm = new SavingsAccountVM(this.dbFilePath);
            //await this.EnsureSystemItemsExistAsync();
            await vm.LoadSavingsAccountDetailsAsync(accountId);

            return vm;
        }

        public async Task<IncomeItemVM> GetIncomeItemVMAsync(int itemId)
        {
            IncomeItemVM vm = new IncomeItemVM(this.dbFilePath);
            //await this.EnsureSystemItemsExistAsync();
            await vm.LoadIncomeItemAsync(itemId);

            return vm;
        }

        public async Task<ExpenseItemVM> GetExpenseItemVMAsync(int itemId)
        {
            ExpenseItemVM vm = new ExpenseItemVM(this.dbFilePath);
            //await this.EnsureSystemItemsExistAsync();
            await vm.LoadExpenseItemAsync(itemId);

            return vm;
        }
    
        public async Task<bool> DeleteBudgetCategoryAsync(BudgetCategory category)
        {
            bool _success = false;  

            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _resultsIncomeItems = await uow.GetCategoryIncomeItemsAsync(category);
                if (_resultsIncomeItems.Successful)
                {
                    foreach(IncomeItem itm in _resultsIncomeItems.Results)
                    {
                        var _resDel = await uow.DeleteIncomeItemAsync(itm);
                    }
                }
                var _resultsExpenseItems = await uow.GetCategoryExpenseItemsAsync(category);
                if (_resultsExpenseItems.Successful)
                {
                    foreach(ExpenseItem itm in _resultsExpenseItems.Results)
                    {
                        var _resDel = await uow.DeleteExpenseItemAsync(itm);
                    }
                }
                var _resultsDeleteCategory = await uow.DeleteBudgetCategoryAsync(category);
                _success = _resultsDeleteCategory.Successful;
            }

            return _success;
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
                        WriteErrorCondition(_results.WorkException.Message);
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
                        WriteErrorCondition(_results.WorkException.Message);
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
    
    public class BudgetItemsVM : BaseViewModel
    {

        public List<BudgetItem> BudgetItems { get; set; }

        public BudgetItemsVM(string dbFilePath)
            : base(dbFilePath)
        {
            BudgetItems = new List<BudgetItem>();
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
                            WriteErrorCondition(_resultsIncomeItems.WorkException.Message);
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
                        WriteErrorCondition(_resultsCategory.WorkException.Message);
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
                            WriteErrorCondition(_resultsIncomeItems.WorkException.Message);
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
                        WriteErrorCondition(_resultsCategory.WorkException.Message);
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

    public class BudgetCategoriesVM : BaseViewModel
    {

        public ICollection<BudgetCategory> BudgetCategories { get; set; }

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
                    //foreach (var category in _results.Results)
                    //{
                    //    this.BudgetCategories.Add(category);
                    //}
                }
                else
                {
                    if (_results.WorkException != null)
                    {
                        WriteErrorCondition(_results.WorkException.Message);
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

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool IsSystemCategory { get; set; }

        public bool IsUserSelected { get; set; }

        public AppIcon CategoryIcon { get; set; }

        public BudgetCategoryType CategoryType { get; set; }

        public ICollection<IncomeItem> IncomeItems { get; set; }

        public ICollection<ExpenseItem> ExpenseItems { get; set; }

        public BudgetCategoryVM(string dbFilePath)
            : base(dbFilePath)
        {
            this.IncomeItems = new List<IncomeItem>();
            this.ExpenseItems = new List<ExpenseItem>();
        }

        internal async Task LoadBudgetCategoryDetails(int categoryId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetBudgetCategoryAsync(categoryId);
                if (_results.Successful)
                {
                    this.Category = _results.Results;
                    this.Name = this.Category?.categoryName ?? string.Empty;
                    this.Description = this.Category?.description ?? string.Empty;
                    this.Amount = this.Category?.budgetAmount ?? 0;
                    this.IsSystemCategory = this.Category?.systemCategory ?? false;
                    this.IsUserSelected = this.Category?.userSelected ?? false;
                    this.CategoryIcon = this.Category?.categoryIcon ?? AppIcon.None;
                    this.CategoryType = this.Category?.categoryType ?? BudgetCategoryType.Expense;

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
                                        this.ExpenseItems.Add(item);
                                    }
                                }
                                else
                                {
                                    if (_resultsExpenseItems.WorkException != null)
                                    {
                                        WriteErrorCondition(_resultsExpenseItems.WorkException.Message);
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
                                        this.IncomeItems.Add(item);
                                    }
                                }
                                else
                                {
                                    if (_resultsIncomeItems.WorkException != null)
                                    {
                                        WriteErrorCondition(_resultsIncomeItems.WorkException.Message);
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
                        WriteErrorCondition(_results.WorkException.Message);
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
                        WriteErrorCondition(_results.WorkException.Message);
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
                        WriteErrorCondition(_results.WorkException.Message);
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
                    this.BudgetedAmount = this.ExpenseItem?.budgetedAmount ?? 0;
                    this.Description = this.ExpenseItem?.description ?? string.Empty;
                    this.Notation = this.ExpenseItem?.notation ?? string.Empty;
                    this.Recurring = this.ExpenseItem?.recurring ?? false;

                }
                else 
                {
                    if (_results.WorkException != null)
                    {
                        WriteErrorCondition(_results.WorkException.Message);
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
                    this.BudgetedAmount = this.IncomeItem?.budgetedAmount ?? 0;
                    this.Description = this.IncomeItem?.description ?? string.Empty;
                    this.Notation = this.IncomeItem?.notation ?? string.Empty;
                    this.Recurring = this.IncomeItem?.recurring ?? false;

                }
                else
                {
                    if (_results.WorkException != null)
                    {
                        WriteErrorCondition(_results.WorkException.Message);
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
    }

}
