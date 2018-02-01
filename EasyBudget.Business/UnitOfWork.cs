using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyBudget.Data;
using EasyBudget.Models;

namespace EasyBudget.Business
{
    public class UnitOfWork : IDisposable
    {
        private IEasyBudgetRepository repository;

        public UnitOfWork(string dbFilePath)
        {
            repository = new EasyBudgetRepository(dbFilePath);
        }

        public UnitOfWork(IEasyBudgetRepository repo)
        {
            repository = repo;
        }

        public void Dispose()
        {
            this.repository?.Dispose();
        }

        public async Task<BudgetCategoryResults> GetBudgetCategoryAsync(Guid id)
        {
            BudgetCategoryResults _results = new BudgetCategoryResults();

            try
            {
                var category = await this.repository.GetBudgetCategoryAsync(id);
                _results.Results = category;
                _results.Successful = category != null;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<BudgetCategoriesResults> GetMatchingBudgetCategories(string searchText)
        {
            BudgetCategoriesResults _results = new BudgetCategoriesResults();

            try
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    throw new Exception("Valid search text must be provided");
                }
                var matches = await this.repository.GetMatchingCategoriesAsync(searchText);
                _results.Results = matches;
                _results.Successful = matches != null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<BudgetCategoryResults> AddBudgetCategoryAsync(BudgetCategory category)
        {
            BudgetCategoryResults _results = new BudgetCategoryResults();

            try
            {
                if (category == null)
                {
                    throw new NullReferenceException("category cannot be NULL");    
                }
                await repository.AddBudgetCategoryAsync(category);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Results = category; 
                _results.Successful = objectsAdded == 1;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<BudgetCategoryResults> UpdateBudgetCategoryAsync(BudgetCategory category)
        {
            BudgetCategoryResults _results = new BudgetCategoryResults();

            try
            {
                if (category == null)
                {
                    throw new NullReferenceException("category cannot be NULL");
                }
                await repository.UpdateBudgetCategoryAsync(category);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = category;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteBudgetCategoryResults> DeleteBudgetCategoryAsync(BudgetCategory category)
        {
            DeleteBudgetCategoryResults _results = new DeleteBudgetCategoryResults();

            try
            {
                if (category == null)
                {
                    throw new NullReferenceException("category cannot be NULL");
                }
                var incomeItms = await repository.GetIncomeItemsForBudgetCategoryAsync(category.id);
                var expenseItms = await repository.GetExpenseItemsForBudgetCategoryAsync(category.id);
                if (incomeItms.Count > 0 || expenseItms.Count > 0)
                {
                    throw new Exception("You must first remove all Income and Expense Items from this Category before deleting it.");
                }
                await repository.DeleteBudgetCategoryAsync(category);
                int objectsRemoved = await this.repository.SaveChangesAsync();
                _results.Results = objectsRemoved == 1;
                _results.Successful = _results.Results;
            }
            catch (Exception ex)
            {
                _results.Results = false;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<ExpenseItemResults> GetExpenseItemAsync(Guid id)
        {
            ExpenseItemResults _results = new ExpenseItemResults();

            try
            {
                ExpenseItem expItm = await repository.GetExpenseItemAsync(id);
                _results.Results = expItm;
                _results.Successful = expItm != null;
                if (expItm == null)
                {
                    _results.Message = "No matching ExpenseItem found";    
                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<BudgetCategoriesResults> GetAllBudgetCategories()
        {
            BudgetCategoriesResults _results = new BudgetCategoriesResults();

            try
            {
                var accounts = await repository.GetAllCategoriesAsync();

                _results.Results = accounts;
                _results.Successful = true;

                if (accounts.Count == 0)
                {
                    _results.Message = "No Budget Categories found";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingAccountsResults> GetAllCheckingAccounts()
        {
            CheckingAccountsResults _results = new CheckingAccountsResults();

            try
            {
                var accounts = await repository.GetAllCheckingAccountsAsync();

                _results.Results = accounts;
                _results.Successful = true;

                if (accounts.Count == 0)
                {
                    _results.Message = "No Checking Accounts found";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;

        }

        public async Task<SavingsAccountsResults> GetAllSavingsAccounts()
        {
            SavingsAccountsResults _results = new SavingsAccountsResults();

            try
            {
                var accounts = await repository.GetAllSavingsAccountsAsync();

                _results.Results = accounts;
                _results.Successful = true;

                if (accounts.Count == 0)
                {
                    _results.Message = "No Savings Accounts found";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;

        }

        public async Task<ExpenseItemsResults> GetCategoryExpenseItemsAsync(BudgetCategory category)
        {
            ExpenseItemsResults _results = new ExpenseItemsResults();

            try
            {
                var items = await repository.GetExpenseItemsForBudgetCategoryAsync(category.id);
                _results.Results = items;
                _results.Successful = items != null;
                if (_results.Results.Count == 0)
                {
                    _results.Message = "No Expense Items found for Budget Category " + category.categoryName;
                }
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<IncomeItemsResults> GetCategoryIncomeItemsAsync(BudgetCategory category)
        {
            IncomeItemsResults _results = new IncomeItemsResults();

            try
            {
                var items = await repository.GetIncomeItemsForBudgetCategoryAsync(category.id);
                _results.Results = items;
                _results.Successful = items != null;
                if (_results.Results.Count == 0)
                {
                    _results.Message = "No Income Items found for Budget Category " + category.categoryName;
                }
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<ExpenseItemResults> AddExpenseItemAsync(ExpenseItem expItem)
        {
            ExpenseItemResults _results = new ExpenseItemResults();

            try
            {
                if (expItem == null)
                {
                    throw new NullReferenceException("Expense Item cannot be NULL");
                }
                expItem.budgetCategory = await repository.GetBudgetCategoryAsync(expItem.budgetCategoryId);
                decimal previousBudgetCategoryAmount = expItem.budgetCategory.budgetAmount;
                _results.PreviousBudgetedAmount = expItem.budgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                decimal newCategoryBudgetAmount = previousBudgetCategoryAmount + expItem.budgetedAmount;
                expItem.budgetCategory.budgetAmount = newCategoryBudgetAmount;

                await repository.AddExpenseItemAsync(expItem);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = expItem;
                _results.BudgetCategory = expItem.budgetCategory;
                _results.BudgetCategoryId = expItem.budgetCategoryId;
                _results.NewBudgetCategoryAmount = newCategoryBudgetAmount;
                _results.NewBudgetedAmount = expItem.budgetedAmount;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<IncomeItemResults> AddIncomeItemAsync(IncomeItem incItem)
        {
            IncomeItemResults _results = new IncomeItemResults();

            try
            {
                if (incItem == null)
                {
                    throw new NullReferenceException("Income Item cannot be NULL");
                }
                incItem.budgetCategory = await repository.GetBudgetCategoryAsync(incItem.budgetCategoryId);
                decimal previousBudgetCategoryAmount = incItem.budgetCategory.budgetAmount;
                _results.PreviousBudgetedAmount = incItem.budgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;

                incItem.budgetCategory.budgetAmount += incItem.budgetedAmount;
                await repository.AddIncomeItemAsync(incItem);
                int objectsAdded = await this.repository.SaveChangesAsync();

                _results.BudgetCategory = incItem.budgetCategory;
                _results.BudgetCategoryId = incItem.budgetCategoryId;
                _results.NewBudgetCategoryAmount = incItem.budgetCategory.budgetAmount;
                _results.NewBudgetedAmount = incItem.budgetedAmount;

                _results.Successful = true;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<ExpenseItemResults> UpdateExpenseItemAsync(ExpenseItem expItem)
        {
            ExpenseItemResults _results = new ExpenseItemResults();

            try
            {
                if (expItem == null)
                {
                    throw new NullReferenceException("Expense Item cannot be NULL");
                }
                // Get the current budget category so we can adjust the budgeted amount
                expItem.budgetCategory = await repository.GetBudgetCategoryAsync(expItem.budgetCategoryId);
                // Existing (previous) category budget total
                decimal previousBudgetCategoryAmount = expItem.budgetCategory.budgetAmount;

                // Get the previous budgeted amount for this item, deduct it from the category 
                // budget amount, then add the new budgeted amount for this item to get the
                // new category total budget amount ExpenseItem existingItem = await repository.GetExpenseItemAsync(expItem.id);
                ExpenseItem existingItem = await repository.GetExpenseItemAsync(expItem.id);
                decimal previousItemBudgetedAmount = existingItem.budgetedAmount;

                _results.PreviousBudgetedAmount = previousItemBudgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                _results.NewBudgetCategoryAmount = previousBudgetCategoryAmount - previousItemBudgetedAmount + expItem.budgetedAmount;
                // Set the new total amount on the budget category
                expItem.budgetCategory.budgetAmount = _results.NewBudgetCategoryAmount;
                // Update the item
                await repository.UpdateExpenseItemAsync(expItem);
                int objectsAdded = await this.repository.SaveChangesAsync();

                // Finish updating the return object
                _results.Successful = true;
                _results.Results = expItem;
                _results.BudgetCategory = expItem.budgetCategory;
                _results.BudgetCategoryId = expItem.budgetCategoryId;
                _results.NewBudgetedAmount = expItem.budgetedAmount;

            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<IncomeItemResults> UpdateIncomeItemAsync(IncomeItem incItem)
        {
            IncomeItemResults _results = new IncomeItemResults();

            try
            {
                if (incItem == null)
                {
                    throw new NullReferenceException("Income Item cannot be NULL");
                }
                // Get the current budget category so we can adjust the budgeted amount
                incItem.budgetCategory = await repository.GetBudgetCategoryAsync(incItem.budgetCategoryId);
                // Existing (previous) category budget total
                decimal previousBudgetCategoryAmount = incItem.budgetCategory.budgetAmount;

                // Get the previous budgeted amount for this item, deduct it from the category 
                // budget amount, then add the new budgeted amount for this item to get the
                // new category total budget amount ExpenseItem existingItem = await repository.GetExpenseItemAsync(expItem.id);
                IncomeItem existingItem = await repository.GetIncomeItemAsync(incItem.id);
                decimal previousItemBudgetedAmount = existingItem.budgetedAmount;

                _results.PreviousBudgetedAmount = previousItemBudgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                _results.NewBudgetCategoryAmount = previousBudgetCategoryAmount - previousItemBudgetedAmount + incItem.budgetedAmount;
                // Set the new total amount on the budget category
                incItem.budgetCategory.budgetAmount = _results.NewBudgetCategoryAmount;
                // Update the item
                await repository.UpdateIncomeItemAsync(incItem);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteExpenseItemResults> DeleteExpenseItemAsync(ExpenseItem expItem)
        {
            DeleteExpenseItemResults _results = new DeleteExpenseItemResults();

            try
            {
                if (expItem == null)
                {
                    throw new NullReferenceException("Expense Item cannot be NULL");
                }
                expItem.budgetCategory = await repository.GetBudgetCategoryAsync(expItem.budgetCategoryId);
                decimal previousBudgetCategoryAmount = expItem.budgetCategory.budgetAmount;
                _results.PreviousBudgetedAmount = expItem.budgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                decimal newCategoryBudgetAmount = previousBudgetCategoryAmount - expItem.budgetedAmount;
                expItem.budgetCategory.budgetAmount = newCategoryBudgetAmount;


                await repository.DeleteExpenseItemAsync(expItem);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;

                _results.BudgetCategory = expItem.budgetCategory;
                _results.BudgetCategoryId = expItem.budgetCategoryId;
                _results.NewBudgetCategoryAmount = newCategoryBudgetAmount;
                _results.NewBudgetedAmount = expItem.budgetedAmount;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteIncomeItemResults> DeleteIncomeItemAsync(IncomeItem incItem)
        {
            DeleteIncomeItemResults _results = new DeleteIncomeItemResults();

            try
            {
                if (incItem == null)
                {
                    throw new NullReferenceException("Income Item cannot be NULL");
                }
                incItem.budgetCategory = await repository.GetBudgetCategoryAsync(incItem.budgetCategoryId);
                decimal previousBudgetCategoryAmount = incItem.budgetCategory.budgetAmount;
                _results.PreviousBudgetedAmount = incItem.budgetedAmount;
                _results.PreviousBudgetCategoryAmount = previousBudgetCategoryAmount;
                decimal newCategoryBudgetAmount = previousBudgetCategoryAmount - incItem.budgetedAmount;
                incItem.budgetCategory.budgetAmount = newCategoryBudgetAmount;


                await repository.DeleteIncomeItemAsync(incItem);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;

                _results.BudgetCategory = incItem.budgetCategory;
                _results.BudgetCategoryId = incItem.budgetCategoryId;
                _results.NewBudgetCategoryAmount = newCategoryBudgetAmount;
                _results.NewBudgetedAmount = incItem.budgetedAmount;

                //await repository.DeleteIncomeItemAsync(incItem);
                //int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = true;
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingAccountResults> GetCheckingAccountAsync(Guid accountId)
        {
            CheckingAccountResults _results = new CheckingAccountResults();

            try
            {
                CheckingAccount account = await repository.GetCheckingAccountAsync(accountId);
                _results.Results = account;
                _results.Successful = account != null;
                if (account == null)
                {
                    _results.Message = "No matching Checking Account found";
                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsAccountResults> GetSavingsAccountAsync(Guid accountId)
        {
            SavingsAccountResults _results = new SavingsAccountResults();

            try
            {
                SavingsAccount account = await repository.GetSavingsAccountAsync(accountId);
                _results.Results = account;
                _results.Successful = account != null;
                if (account == null)
                {
                    _results.Message = "No matching Savings Account found";
                }
                _results.WorkException = null;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingAccountResults> AddCheckingAccountAsync(CheckingAccount account)
        {
            CheckingAccountResults _results = new CheckingAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Checking Account cannot be NULL");
                }
                await repository.AddCheckingAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = account;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsAccountResults> AddSavingsAccountAsync(SavingsAccount account)
        {
            SavingsAccountResults _results = new SavingsAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Savings Account cannot be NULL");
                }
                await repository.AddSavingsAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = account;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingAccountResults> UpdateCheckingAccountAsync(CheckingAccount account)
        {
            CheckingAccountResults _results = new CheckingAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Checking Account cannot be NULL");
                }
                await repository.UpdateCheckingAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = account;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<SavingsAccountResults> UpdateSavingsAccountAsync(SavingsAccount account)
        {
            SavingsAccountResults _results = new SavingsAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Savings Account cannot be NULL");
                }
                await repository.UpdateSavingsAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Successful = objectsAdded == 1;
                _results.Results = account;
            }
            catch (Exception ex)
            {
                _results.Results = null;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteCheckingAccountResults> DeleteCheckingAccountAsync(CheckingAccount account)
        {
            DeleteCheckingAccountResults _results = new DeleteCheckingAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Checking Account cannot be NULL");
                }
                await repository.DeleteCheckingAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Results = objectsAdded == 1;
                _results.Successful = _results.Results;
            }
            catch (Exception ex)
            {
                _results.Results = false;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<DeleteSavingsAccountResults> DeleteSavingsAccountAsync(SavingsAccount account)
        {
            DeleteSavingsAccountResults _results = new DeleteSavingsAccountResults();

            try
            {
                if (account == null)
                {
                    throw new NullReferenceException("Savings Account cannot be NULL");
                }
                await repository.DeleteSavingsAccountAsync(account);
                int objectsAdded = await this.repository.SaveChangesAsync();
                _results.Results = objectsAdded == 1;
                _results.Successful = true;
            }
            catch (Exception ex)
            {
                _results.Results = false;
                _results.Successful = false;
                _results.WorkException = ex;
            }

            return _results;
        }

        public async Task<CheckingAccountWithdrawalResults> SpendMoneyCheckingAsync(CheckingWithdrawal withdrawal)
        {
            CheckingAccountWithdrawalResults _results = new CheckingAccountWithdrawalResults();

            try
            {
                if (withdrawal.checkingAccountId == Guid.Empty)
                {
                    throw new Exception("The Checking Account ID is required with all transactions.");
                }
                if (withdrawal.checkingAccount == null)
                {
                    withdrawal.checkingAccount = await repository.GetCheckingAccountAsync(withdrawal.checkingAccountId);
                    if (withdrawal.checkingAccount == null)
                    {
                        throw new Exception("Unable to locate the Checking Account associated with this record.");
                    }
                }
                // Get the most current values for the related account
                withdrawal.checkingAccount = await repository.GetCheckingAccountAsync(withdrawal.checkingAccountId);
                withdrawal.checkingAccount.currentBalance = withdrawal.checkingAccount.currentBalance - withdrawal.transactionAmount;
                await repository.AddCheckingWithdrawalAsync(withdrawal);
                await repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = withdrawal;
                _results.EndingAccountBalance = withdrawal.checkingAccount.currentBalance;
                _results.AccountId = withdrawal.checkingAccount.id;
                _results.Account = withdrawal.checkingAccount;
                _results.TransactionAmount = withdrawal.transactionAmount;
            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        public async Task<CheckingAccountDepositResults> DepositMoneyCheckingAsync(CheckingDeposit deposit)
        {
            CheckingAccountDepositResults _results = new CheckingAccountDepositResults();

            try
            {
                if (deposit.checkingAccountId == Guid.Empty)
                {
                    throw new Exception("The Checking Account ID is required with all transactions.");
                }
                if (deposit.checkingAccount == null)
                {
                    deposit.checkingAccount = await repository.GetCheckingAccountAsync(deposit.checkingAccountId);
                    if (deposit.checkingAccount == null)
                    {
                        throw new Exception("Unable to locate the Checking Account associated with this record.");
                    }
                }
                // Get the most current values for the related account:
                deposit.checkingAccount = await repository.GetCheckingAccountAsync(deposit.checkingAccountId);
                deposit.checkingAccount.currentBalance = deposit.checkingAccount.currentBalance + deposit.transactionAmount;
                await repository.AddCheckingDepositAsync(deposit);
                await repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = deposit;
                _results.EndingAccountBalance = deposit.checkingAccount.currentBalance;
                _results.AccountId = deposit.checkingAccount.id;
                _results.Account = deposit.checkingAccount;
                _results.TransactionAmount = deposit.transactionAmount;
            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        public async Task<SavingsAccountWithdrawalResults> SpendMoneySavingsAsync(SavingsWithdrawal withdrawal)
        {
            SavingsAccountWithdrawalResults _results = new SavingsAccountWithdrawalResults();

            try
            {
                if (withdrawal.savingsAccountId == Guid.Empty)
                {
                    throw new Exception("The Savings Account ID is required with all transactions.");
                }
                if (withdrawal.savingsAccount == null)
                {
                    withdrawal.savingsAccount = await repository.GetSavingsAccountAsync(withdrawal.savingsAccountId);
                    if (withdrawal.savingsAccount == null)
                    {
                        throw new Exception("Unable to locate the Savings Account associated with this record.");
                    }
                }
                withdrawal.savingsAccount.currentBalance = withdrawal.savingsAccount.currentBalance - withdrawal.transactionAmount;
                await repository.AddSavingsWithdrawalAsync(withdrawal);
                await repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = withdrawal;
                _results.EndingAccountBalance = withdrawal.savingsAccount.currentBalance;
                _results.AccountId = withdrawal.savingsAccount.id;
                _results.Account = withdrawal.savingsAccount;
                _results.TransactionAmount = withdrawal.transactionAmount;
            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        public async Task<SavingsAccountDepositResults> DepositMoneySavingsAsync(SavingsDeposit deposit)
        {
            SavingsAccountDepositResults _results = new SavingsAccountDepositResults();

            try
            {
                if (deposit.savingsAccountId == Guid.Empty)
                {
                    throw new Exception("The Savings Account ID is required with all transactions.");
                }
                if (deposit.savingsAccount == null)
                {
                    deposit.savingsAccount = await repository.GetSavingsAccountAsync(deposit.savingsAccountId);
                    if (deposit.savingsAccount == null)
                    {
                        throw new Exception("Unable to locate the Savings Account associated with this record.");
                    }
                }
                deposit.savingsAccount.currentBalance = deposit.savingsAccount.currentBalance + deposit.transactionAmount;
                await repository.AddSavingsDepositAsync(deposit);
                await repository.SaveChangesAsync();
                _results.Successful = true;
                _results.Results = deposit;
                _results.EndingAccountBalance = deposit.savingsAccount.currentBalance;
                _results.AccountId = deposit.savingsAccount.id;
                _results.Account = deposit.savingsAccount;
                _results.TransactionAmount = deposit.transactionAmount;
            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        public async Task<FundsTransferResults> TransferMoneyAsync(BankAccountFundsTransfer fundsTransfer)
        {
            FundsTransferResults _results = new FundsTransferResults();

            try
            {
                // Validate the Funds Transfer object:
                if (fundsTransfer.sourceAccountId == Guid.Empty)
                {
                    throw new Exception("The Source Account must be correctly identified");
                }
                if (fundsTransfer.destinationAccountId == Guid.Empty)
                {
                    throw new Exception("The Destination Account must be correctly identified");
                }
                if (fundsTransfer.transactionAmount <= 0)
                {
                    throw new Exception("The Transaction Amount must be greater than $0.00");
                }

                // Initiate and persist the data:
                CheckingAccount sourceChecking = null;
                CheckingAccount destinationChecking = null;
                SavingsAccount sourceSavings = null;
                SavingsAccount destinationSavings = null;

                switch (fundsTransfer.sourceAccountType)
                {
                    case BankAccountType.Checking:
                        sourceChecking = await repository.GetCheckingAccountAsync(fundsTransfer.sourceAccountId);
                        if (sourceChecking == null)
                        {
                            throw new Exception("Unable to locate the Source Checking Account");
                        }
                        fundsTransfer.sourceAccountBeginningBalance = sourceChecking.currentBalance;
                        fundsTransfer.sourceAccountEndingBalance = fundsTransfer.sourceAccountBeginningBalance - fundsTransfer.transactionAmount;
                        sourceChecking.currentBalance = fundsTransfer.sourceAccountEndingBalance;
                        fundsTransfer.sourceAccount = sourceChecking;
                        await repository.UpdateCheckingAccountAsync(sourceChecking);
                        break;
                    case BankAccountType.Savings:
                        sourceSavings = await repository.GetSavingsAccountAsync(fundsTransfer.sourceAccountId);
                        if (sourceSavings == null) 
                        {
                            throw new Exception("Unable to locate the Source Savings Account");
                        }
                        fundsTransfer.sourceAccountBeginningBalance = sourceSavings.currentBalance;
                        fundsTransfer.sourceAccountEndingBalance = fundsTransfer.sourceAccountBeginningBalance - fundsTransfer.transactionAmount;
                        sourceSavings.currentBalance = fundsTransfer.sourceAccountEndingBalance;
                        fundsTransfer.sourceAccount = sourceSavings;
                        await repository.UpdateSavingsAccountAsync(sourceSavings);
                        break;
                }

                switch (fundsTransfer.destinationAccountType)
                {
                    case BankAccountType.Checking:
                        destinationChecking = await repository.GetCheckingAccountAsync(fundsTransfer.destinationAccountId);
                        if (destinationChecking == null)
                        {
                            throw new Exception("Unable to locate the Destination Checking Account");
                        }
                        fundsTransfer.destinationAccountBeginningBalance = destinationChecking.currentBalance;
                        fundsTransfer.destinationAccountEndingBalance = fundsTransfer.destinationAccountBeginningBalance + fundsTransfer.transactionAmount;
                        destinationChecking.currentBalance = fundsTransfer.destinationAccountEndingBalance;
                        fundsTransfer.destinationAccount = destinationChecking;
                        await repository.UpdateCheckingAccountAsync(destinationChecking);
                        break;
                    case BankAccountType.Savings:
                        destinationSavings = await repository.GetSavingsAccountAsync(fundsTransfer.destinationAccountId);
                        if (destinationSavings == null) 
                        {
                            throw new Exception("Unable to locate the Destination Savings Account");
                        }
                        fundsTransfer.destinationAccountBeginningBalance = destinationSavings.currentBalance;
                        fundsTransfer.destinationAccountEndingBalance = fundsTransfer.destinationAccountBeginningBalance + fundsTransfer.transactionAmount;
                        destinationSavings.currentBalance = fundsTransfer.destinationAccountEndingBalance;
                        fundsTransfer.destinationAccount = destinationSavings;
                        await repository.UpdateSavingsAccountAsync(destinationSavings);
                        break;
                }

                await repository.AddBankAccountFundsTransferAsync(fundsTransfer);
                await repository.SaveChangesAsync();
                _results.Successful = true;

                _results.destinationAccount = fundsTransfer.destinationAccount;
                _results.destinationAccountId = fundsTransfer.destinationAccountId;
                _results.destinationAccountType = fundsTransfer.destinationAccountType;
                _results.destinationAccountBeginningBalance = fundsTransfer.destinationAccountBeginningBalance;
                _results.destinationAccountEndingBalance = fundsTransfer.destinationAccountEndingBalance;

                _results.sourceAccount = fundsTransfer.sourceAccount;
                _results.sourceAccountId = fundsTransfer.sourceAccountId;
                _results.sourceAccountType = fundsTransfer.sourceAccountType;
                _results.sourceAccountBeginningBalance = fundsTransfer.sourceAccountBeginningBalance;
                _results.sourceAccountEndingBalance = fundsTransfer.sourceAccountEndingBalance;

            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }

        public async Task<FundsTransferResults> VoidFundsTransferAsync(BankAccountFundsTransfer fundsTransfer)
        {
            FundsTransferResults _results = new FundsTransferResults();

            try
            {
                // Validate the Funds Transfer object:
                if (fundsTransfer.sourceAccountId == Guid.Empty)
                {
                    throw new Exception("The Source Account must be correctly identified");
                }
                if (fundsTransfer.destinationAccountId == Guid.Empty)
                {
                    throw new Exception("The Destination Account must be correctly identified");
                }
                if (fundsTransfer.transactionAmount <= 0)
                {
                    throw new Exception("The Transaction Amount must be greater than $0.00");
                }

                // Initiate and persist the data:
                CheckingAccount sourceChecking = null;
                CheckingAccount destinationChecking = null;
                SavingsAccount sourceSavings = null;
                SavingsAccount destinationSavings = null;

                switch (fundsTransfer.sourceAccountType)
                {
                    case BankAccountType.Checking:
                        sourceChecking = await repository.GetCheckingAccountAsync(fundsTransfer.sourceAccountId);
                        if (sourceChecking == null)
                        {
                            throw new Exception("Unable to locate the Source Checking Account");
                        }
                        fundsTransfer.sourceAccountBeginningBalance = sourceChecking.currentBalance;
                        fundsTransfer.sourceAccountEndingBalance = fundsTransfer.sourceAccountBeginningBalance + fundsTransfer.transactionAmount;
                        sourceChecking.currentBalance = fundsTransfer.sourceAccountEndingBalance;
                        fundsTransfer.sourceAccount = sourceChecking;
                        await repository.UpdateCheckingAccountAsync(sourceChecking);
                        break;
                    case BankAccountType.Savings:
                        sourceSavings = await repository.GetSavingsAccountAsync(fundsTransfer.sourceAccountId);
                        if (sourceSavings == null)
                        {
                            throw new Exception("Unable to locate the Source Savings Account");
                        }
                        fundsTransfer.sourceAccountBeginningBalance = sourceSavings.currentBalance;
                        fundsTransfer.sourceAccountEndingBalance = fundsTransfer.sourceAccountBeginningBalance + fundsTransfer.transactionAmount;
                        sourceSavings.currentBalance = fundsTransfer.sourceAccountEndingBalance;
                        fundsTransfer.sourceAccount = sourceSavings;
                        await repository.UpdateSavingsAccountAsync(sourceSavings);
                        break;
                }

                switch (fundsTransfer.destinationAccountType)
                {
                    case BankAccountType.Checking:
                        destinationChecking = await repository.GetCheckingAccountAsync(fundsTransfer.destinationAccountId);
                        if (destinationChecking == null)
                        {
                            throw new Exception("Unable to locate the Destination Checking Account");
                        }
                        fundsTransfer.destinationAccountBeginningBalance = destinationChecking.currentBalance;
                        fundsTransfer.destinationAccountEndingBalance = fundsTransfer.destinationAccountBeginningBalance - fundsTransfer.transactionAmount;
                        destinationChecking.currentBalance = fundsTransfer.destinationAccountEndingBalance;
                        fundsTransfer.destinationAccount = destinationChecking;
                        await repository.UpdateCheckingAccountAsync(destinationChecking);
                        break;
                    case BankAccountType.Savings:
                        destinationSavings = await repository.GetSavingsAccountAsync(fundsTransfer.destinationAccountId);
                        if (destinationSavings == null)
                        {
                            throw new Exception("Unable to locate the Destination Savings Account");
                        }
                        fundsTransfer.destinationAccountBeginningBalance = destinationSavings.currentBalance;
                        fundsTransfer.destinationAccountEndingBalance = fundsTransfer.destinationAccountBeginningBalance - fundsTransfer.transactionAmount;
                        destinationSavings.currentBalance = fundsTransfer.destinationAccountEndingBalance;
                        fundsTransfer.destinationAccount = destinationSavings;
                        await repository.UpdateSavingsAccountAsync(destinationSavings);
                        break;
                }

                await repository.UpdateBankAccountFundsTransferAsync(fundsTransfer);
                await repository.SaveChangesAsync();
                _results.Successful = true;

                _results.destinationAccount = fundsTransfer.destinationAccount;
                _results.destinationAccountId = fundsTransfer.destinationAccountId;
                _results.destinationAccountType = fundsTransfer.destinationAccountType;
                _results.destinationAccountBeginningBalance = fundsTransfer.destinationAccountBeginningBalance;
                _results.destinationAccountEndingBalance = fundsTransfer.destinationAccountEndingBalance;

                _results.sourceAccount = fundsTransfer.sourceAccount;
                _results.sourceAccountId = fundsTransfer.sourceAccountId;
                _results.sourceAccountType = fundsTransfer.sourceAccountType;
                _results.sourceAccountBeginningBalance = fundsTransfer.sourceAccountBeginningBalance;
                _results.sourceAccountEndingBalance = fundsTransfer.sourceAccountEndingBalance;

            }
            catch (Exception ex)
            {
                _results.WorkException = ex;
                _results.Successful = false;
            }

            return _results;
        }
    }
}
