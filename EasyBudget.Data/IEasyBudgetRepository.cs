using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyBudget.Models.DataModels;

namespace EasyBudget.Data
{
    public interface IEasyBudgetRepository : IDisposable
    {
        Task<BudgetCategory> GetBudgetCategoryAsync(Guid id);

        Task<ICollection<BudgetCategory>> GetAllCategoriesAsync();

        Task<ICollection<BudgetCategory>> GetMatchingCategoriesAsync(string searchText);

        Task<CheckingAccount> GetCheckingAccountAsync(Guid id);

        Task<ICollection<CheckingAccount>> GetAllCheckingAccountsAsync();

        Task<CheckingDeposit> GetCheckingDepositAsync(Guid id);

        Task<CheckingWithdrawal> GetCheckingWithdrawalAsync(Guid id);

        Task<ICollection<CheckingDeposit>> GetCheckingDepositsByDateRangeAsync(Guid accountId, DateTime fromDate, DateTime toDate);

        Task<ICollection<CheckingWithdrawal>> GetCheckingWithdrawalsByDateRangeAsync(Guid accountId, DateTime fromDate, DateTime toDate);

        Task<SavingsAccount> GetSavingsAccountAsync(Guid id);

        Task<ICollection<SavingsAccount>> GetAllSavingsAccountsAsync();

        Task<SavingsDeposit> GetSavingsDepositAsync(Guid id);

        Task<SavingsWithdrawal> GetSavingsWithdrawalAsync(Guid id);

        Task<ICollection<SavingsDeposit>> GetSavingsDepositsByDateRangeAsync(Guid accountId, DateTime fromDate, DateTime toDate);

        Task<ICollection<SavingsWithdrawal>> GetSavingsWithdrawalsByDateRangeAsync(Guid accountId, DateTime fromDate, DateTime toDate);

        Task<ExpenseItem> GetExpenseItemAsync(Guid id);

        Task<ICollection<IncomeItem>> GetIncomeItemsForBudgetCategoryAsync(Guid categoryId);

        Task<IncomeItem> GetIncomeItemAsync(Guid id);

        Task<ICollection<ExpenseItem>> GetExpenseItemsForBudgetCategoryAsync(Guid categoryId);

        Task AddCheckingAccountAsync(CheckingAccount account);

        Task AddSavingsAccountAsync(SavingsAccount account);

        Task AddBudgetCategoryAsync(BudgetCategory category);

        Task AddCheckingDepositAsync(CheckingDeposit deposit);

        Task AddCheckingWithdrawalAsync(CheckingWithdrawal withdrawal);

        Task AddSavingsWithdrawalAsync(SavingsWithdrawal withdrawal);

        Task AddSavingsDepositAsync(SavingsDeposit deposit);

        Task AddExpenseItemAsync(ExpenseItem expense);

        Task AddIncomeItemAsync(IncomeItem income);

        Task DeleteCheckingAccountAsync(CheckingAccount account);

        Task DeleteSavingsAccountAsync(SavingsAccount account);

        Task DeleteBudgetCategoryAsync(BudgetCategory category);

        Task DeleteCheckingDepositAsync(CheckingDeposit deposit);

        Task DeleteCheckingWithdrawalAsync(CheckingWithdrawal withdrawal);

        Task DeleteSavingsDepositAsync(SavingsDeposit deposit);

        Task DeleteExpenseItemAsync(ExpenseItem expense);

        Task DeleteIncomeItemAsync(IncomeItem income);

        Task UpdateCheckingAccountAsync(CheckingAccount account);

        Task UpdateSavingsAccountAsync(SavingsAccount account);

        Task UpdateBudgetCategoryAsync(BudgetCategory category);

        Task UpdateCheckingDepositAsync(CheckingDeposit deposit);

        Task UpdateCheckingWithdrawalAsync(CheckingWithdrawal withdrawal);

        Task UpdateSavingsDepositAsync(SavingsDeposit deposit);

        Task UpdateExpenseItemAsync(ExpenseItem expense);

        Task UpdateIncomeItemAsync(IncomeItem income);

        Task<BankAccountFundsTransfer> GetBankAccountFundsTransfer(Guid transferId);

        Task AddBankAccountFundsTransferAsync(BankAccountFundsTransfer fundsTransfer);

        Task UpdateBankAccountFundsTransferAsync(BankAccountFundsTransfer fundsTransfer);

        Task<int> SaveChangesAsync();
    }
}
