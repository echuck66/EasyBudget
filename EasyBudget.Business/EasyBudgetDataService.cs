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

        public async Task<BankAccountsVM> GetBankAccountsViewModelAsync()
        {
            BankAccountsVM vm = new BankAccountsVM(this.dbFilePath);
            await vm.LoadCheckingAccountsAsync();
            await vm.LoadSavingsAccountsAsync();

            return vm;
        }

        public async Task<BudgetCategoriesVM> GetBudgetCategoriesViewModelAsync()
        {
            BudgetCategoriesVM vm = new BudgetCategoriesVM(this.dbFilePath);
            await vm.LoadBudgetCategoriesAsync();

            return vm;
        }

        public async Task<BudgetCategoryVM> GetBudgetCategoryVM(Guid categoryId)
        {
            BudgetCategoryVM vm = new BudgetCategoryVM(this.dbFilePath);
            await vm.LoadBudgetCategoryDetails(categoryId);

            return vm;
        }

        public async Task<CheckingAccountVM> GetCheckingAccountVMAsync(Guid accountId)
        {
            CheckingAccountVM vm = new CheckingAccountVM(this.dbFilePath);
            await vm.LoadCheckingAccountDetailsAsync(accountId);

            return vm;
        }

        public async Task<SavingsAccountVM> GetSavingsAccountVMAsync(Guid accountId)
        {
            SavingsAccountVM vm = new SavingsAccountVM(this.dbFilePath);
            await vm.LoadSavingsAccountDetailsAsync(accountId);

            return vm;
        }

        public async Task<IncomeItemsVM> GetIncomeItemsVMAsync(Guid categoryId)
        {
            IncomeItemsVM vm = new IncomeItemsVM(this.dbFilePath);
            await vm.LoadIncomeItemsAsync(categoryId);

            return vm;
        }

        public async Task<ExpenseItemsVM> GetExpenseItemsVMAsync(Guid categoryId)
        {
            ExpenseItemsVM vm = new ExpenseItemsVM(this.dbFilePath);
            await vm.LoadExpenseItemsAsync(categoryId);

            return vm;
        }
    
        public async Task<IncomeItemVM> GetIncomeItemVMAsync(Guid itemId)
        {
            IncomeItemVM vm = new IncomeItemVM(this.dbFilePath);
            await vm.LoadIncomeItemAsync(itemId);

            return vm;
        }

        public async Task<ExpenseItemVM> GetExpenseItemVMAsync(Guid itemId)
        {
            ExpenseItemVM vm = new ExpenseItemVM(this.dbFilePath);
            await vm.LoadExpenseItemAsync(itemId);

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

        public void WriteErrorCondition(string error)
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

    public class BankAccountsVM : BaseViewModel, INotifyPropertyChanged
    {

        public ObservableCollection<CheckingAccount> CheckingAccounts { get; set; }

        public ObservableCollection<SavingsAccount> SavingsAccounts { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BankAccountsVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public async Task LoadCheckingAccountsAsync()
        {
            this.CheckingAccounts = new ObservableCollection<CheckingAccount>();
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllCheckingAccountsAsync();
                if (_results.Successful)
                {
                    foreach (var account in _results.Results)
                    {
                        this.CheckingAccounts.Add(account);
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

        public async Task LoadSavingsAccountsAsync()
        {
            this.SavingsAccounts = new ObservableCollection<SavingsAccount>();
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllSavingsAccountsAsync();
                if (_results.Successful)
                {
                    foreach (var account in _results.Results)
                    {
                        this.SavingsAccounts.Add(account);
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

    public class BudgetCategoriesVM : BaseViewModel, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<BudgetCategory> BudgetCategories { get; set; }

        public BudgetCategoriesVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public async Task LoadBudgetCategoriesAsync()
        {
            this.BudgetCategories = new ObservableCollection<BudgetCategory>();
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetAllBudgetCategoriesAsync();
                if (_results.Successful)
                {
                    foreach (var category in _results.Results)
                    {
                        this.BudgetCategories.Add(category);
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

    public class BudgetCategoryVM : BaseViewModel, INotifyPropertyChanged
    {
        BudgetCategory Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool IsSystemCategory { get; set; }

        public bool IsUserSelected { get; set; }

        public AppIcon CategoryIcon { get; set; }

        public BudgetCategoryType CategoryType { get; set; }

        public ObservableCollection<IncomeItem> IncomeItems { get; set; }

        public ObservableCollection<ExpenseItem> ExpenseItems { get; set; }

        public BudgetCategoryVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public async Task LoadBudgetCategoryDetails(Guid categoryId)
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
                                    this.ExpenseItems = new ObservableCollection<ExpenseItem>();
                                    foreach(var item in _resultsExpenseItems.Results)
                                    {
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
                                    this.IncomeItems = new ObservableCollection<IncomeItem>();
                                    foreach(var item in _resultsIncomeItems.Results)
                                    {
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

    public class CheckingAccountVM : BaseViewModel, INotifyPropertyChanged
    {
        CheckingAccount CheckingAccount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string RoutingNumber { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string AccountNickname { get; set; }

        public decimal CurrentBalance { get; set; }

        public DateTime LoadTransactionsFromDate { get; set; }

        public DateTime LoadTransactionsToDate { get; set; }

        public ObservableCollection<CheckingWithdrawal> Withdrawals { get; set; }

        public ObservableCollection<CheckingDeposit> Deposits { get; set; }

        public CheckingAccountVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public async Task LoadCheckingAccountDetailsAsync(Guid accountId)
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

    public class SavingsAccountVM : BaseViewModel, INotifyPropertyChanged
    {
        SavingsAccount SavingsAccount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string RoutingNumber { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string AccountNickname { get; set; }

        public decimal CurrentBalance { get; set; }

        public SavingsAccountVM(string dbFilePath)
            : base(dbFilePath)
        {

        }

        public async Task LoadSavingsAccountDetailsAsync(Guid accountId)
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

    public class IncomeItemsVM : BaseViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<IncomeItem> IncomeItems { get; set; }

        public IncomeItemsVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public async Task LoadIncomeItemsAsync(Guid categoryId)
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
                        this.IncomeItems = new ObservableCollection<IncomeItem>();
                        foreach (var item in _resultsIncomeItems.Results)
                        {
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
    }

    public class ExpenseItemsVM : BaseViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ExpenseItem> ExpenseItems { get; set; }

        public ExpenseItemsVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public async Task LoadExpenseItemsAsync(Guid categoryId)
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
                        this.ExpenseItems = new ObservableCollection<ExpenseItem>();
                        foreach (var item in _resultsIncomeItems.Results)
                        {
                            this.ExpenseItems.Add(item);
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

    public class ExpenseItemVM : BaseViewModel, INotifyPropertyChanged
    {
        
        ExpenseItem ExpenseItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal BudgetedAmount { get; set; }

        public string Description { get; set; }

        public string Notation { get; set; }

        public bool Recurring { get; set; }

        public ExpenseItemVM(string dbFilePath)
            : base(dbFilePath)
        {
            
        }

        public async Task LoadExpenseItemAsync(Guid itemId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetExpenseItemAsync(itemId);
                if (_results.Successful)
                {
                    this.ExpenseItem = _results.Results;

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

    public class IncomeItemVM : BaseViewModel, INotifyPropertyChanged
    {
        IncomeItem IncomeItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal BudgetedAmount { get; set; }

        public string Description { get; set; }

        public string Notation { get; set; }

        public bool Recurring { get; set; }

        public IncomeItemVM(string dbFilePath)
            : base(dbFilePath)
        {

        }

        public async Task LoadIncomeItemAsync(Guid itemId)
        {
            using (UnitOfWork uow = new UnitOfWork(this.dbFilePath))
            {
                var _results = await uow.GetIncomeItemAsync(itemId);
                if (_results.Successful)
                {
                    this.IncomeItem = _results.Results;

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
