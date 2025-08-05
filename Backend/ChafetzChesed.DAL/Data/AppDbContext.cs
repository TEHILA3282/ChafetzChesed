using Microsoft.EntityFrameworkCore;
using ChafetzChesed.DAL.Entities;
namespace ChafetzChesed.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Registration> Registrations { get; set; }
        public DbSet<DepositType> DepositTypes { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<AccountAction> AccountActions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>().ToTable("Registration");
            base.OnModelCreating(modelBuilder);
        }
    }
}
