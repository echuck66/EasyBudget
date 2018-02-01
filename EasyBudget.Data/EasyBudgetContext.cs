using EasyBudget.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace EasyBudget.Data
{
    public class EasyBudgetContext : DbContext
    {
        private string _dbFilePath;

        public DbSet<BudgetCategory> BudgetCategory { get; set; }

        public DbSet<CheckingAccount> CheckingAccount { get; set; }

        public DbSet<CheckingDeposit> CheckingDeposit { get; set; }

        public DbSet<CheckingWithdrawal> CheckingWithdrawal { get; set; }

        public DbSet<ExpenseItem> ExpenseItem { get; set; }

        public DbSet<IncomeItem> IncomeItem { get; set; }

        public DbSet<SavingsAccount> SavingsAccount { get; set; }

        public DbSet<SavingsDeposit> SavingsDeposit { get; set; }

        public DbSet<SavingsWithdrawal> SavingsWithdrawal { get; set; }

        public DbSet<BankAccountFundsTransfer> BankAccountFundsTransfer { get; set; }

        public EasyBudgetContext(string dbFilePath)
        {
            _dbFilePath = dbFilePath;
            Database.EnsureCreated();
        }

        // Used for Unit Testing
        public EasyBudgetContext(DbContextOptions<EasyBudgetContext> options)
            : base(options)
        { 
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename ={_dbFilePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
        }
    }
}
