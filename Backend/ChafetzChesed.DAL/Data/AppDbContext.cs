using Microsoft.EntityFrameworkCore;
using ChafetzChesed.DAL.Entities;
using System.Collections.Generic;

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
    }
}
