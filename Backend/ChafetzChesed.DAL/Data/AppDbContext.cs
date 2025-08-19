using Microsoft.EntityFrameworkCore;
using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Registration> Registrations { get; set; }
        public DbSet<DepositType> DepositTypes { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<AccountAction> AccountActions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<LoanGuarantor> LoanGuarantors { get; set; }
        public DbSet<FreezeRequest> FreezeRequests { get; set; }
        public DbSet<DepositWithdrawRequest> DepositWithdrawRequests { get; set; }
        public DbSet<Institution> Institutions { get; set; } = default!;
        public DbSet<SearchIndexItem> SearchIndexItem { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>().ToTable("Registration");

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.LoanType)
                      .WithMany()
                      .HasForeignKey(e => e.LoanTypeID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Client)
                      .WithMany()
                      .HasForeignKey(e => e.ClientID)
                      .HasPrincipalKey(r => r.ID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LoanGuarantor>(entity =>
            {
                entity.ToTable("LoanGuarantors");

                entity.Property(g => g.IdNumber).HasMaxLength(20).IsRequired();
                entity.Property(g => g.FullName).HasMaxLength(200).IsRequired();
                entity.Property(g => g.Phone).HasMaxLength(30).IsRequired();

                entity.HasOne(g => g.Loan)
                      .WithMany(l => l.Guarantors)
                      .HasForeignKey(g => g.LoanId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(g => new { g.LoanId, g.IdNumber }).IsUnique();
            });
            modelBuilder.Entity<FreezeRequest>(entity =>
            {
                entity.ToTable("FreezeRequests");

                entity.HasKey(f => f.ID);
                entity.Property(f => f.ID).UseIdentityColumn();

                entity.Property(f => f.ClientID)
                      .HasMaxLength(9)        
                      .IsRequired();

                entity.Property(f => f.InstitutionId).IsRequired();

                entity.Property(f => f.RequestType)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(f => f.Reason)
                      .HasMaxLength(1000)
                      .IsRequired();

                entity.Property(f => f.Acknowledged)
                      .HasDefaultValue(false);

                entity.Property(f => f.CreatedAt)
                      .HasColumnType("datetime2(0)")
                      .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.HasOne(f => f.Client)
                      .WithMany()
                      .HasForeignKey(f => f.ClientID)
                      .HasPrincipalKey(r => r.ID)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasCheckConstraint("CK_FreezeRequests_RequestType",
                    "[RequestType] IN (N'loan', N'deposit')");
            });




            base.OnModelCreating(modelBuilder);
        }
    }
}
