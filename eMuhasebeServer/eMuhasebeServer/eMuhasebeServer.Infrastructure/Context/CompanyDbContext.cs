using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enum;
using eMuhasebeServer.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eMuhasebeServer.Infrastructure.Context
{
    internal sealed class CompanyDbContext : DbContext, IUnitOfWorkCompany
    {

        public DbSet<CashRegister> CashRegisters { get; set; }
        public DbSet<CashRegisterDetail> CashRegisterDetails { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankDetail> BankDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }

        #region Connection Part

        private string connectionString = string.Empty;
        public CompanyDbContext(Company company)
        {
            CreateConnectionStringWithCompany(company);
        }

        public CompanyDbContext(IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            CreateConnectionString(httpContextAccessor, applicationDbContext);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        private void CreateConnectionString(IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            if (httpContextAccessor.HttpContext is null) return;

            string? companyId = httpContextAccessor.HttpContext.User.FindFirstValue("companyId");

            if (string.IsNullOrEmpty(companyId)) return;

            Company? company = applicationDbContext.Companies.Find(Guid.Parse(companyId));

            if (company == null) return;

            CreateConnectionStringWithCompany(company);
        }

        private void CreateConnectionStringWithCompany(Company company)
        {
            connectionString =
                $"Server={company.Database.Server};" +
                $"Database={company.Database.DatabaseName};" +
                $"User Id=sa;" +
                $"Password=admin;" +
                "TrustServerCertificate=True;" +
                "Encrypt=False;";
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Table Names

            modelBuilder.Entity<CashRegister>().ToTable("CashRegisters");
            modelBuilder.Entity<CashRegisterDetail>().ToTable("CashRegisterDetails");
            modelBuilder.Entity<Bank>().ToTable("Banks");
            modelBuilder.Entity<BankDetail>().ToTable("BankDetails");
            modelBuilder.Entity<Customer>().ToTable("Customers");

            #endregion

            #region CashRegister ModelBuilder
            modelBuilder.Entity<CashRegister>().Property(p => p.CashDepositAmount).HasColumnType("money");
            modelBuilder.Entity<CashRegister>().Property(p => p.CashWithdrawalAmount).HasColumnType("money");
            modelBuilder.Entity<CashRegister>().Property(p => p.CurrencyType)
                .HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
            modelBuilder.Entity<CashRegister>().HasQueryFilter(filter => !filter.IsDeleted);
            modelBuilder.Entity<CashRegister>()
                .HasMany(p => p.CashRegisterDetails)
                .WithOne()
                .HasForeignKey(p => p.CashRegisterId);
            #endregion

            #region CashRegisterDetail ModelBuilder
            modelBuilder.Entity<CashRegisterDetail>().Property(p => p.CashDepositAmount).HasColumnType("money");
            modelBuilder.Entity<CashRegisterDetail>().Property(p => p.CashWithdrawalAmount).HasColumnType("money");
            modelBuilder.Entity<CashRegisterDetail>().HasQueryFilter(filter => !filter.IsDeleted);
            #endregion

            #region Bank ModelBuilder
            modelBuilder.Entity<Bank>().Property(p => p.BankDepositAmount).HasColumnType("money");
            modelBuilder.Entity<Bank>().Property(p => p.BankWithdrawalAmount).HasColumnType("money");
            modelBuilder.Entity<Bank>().Property(p => p.CurrencyType)
                .HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
            modelBuilder.Entity<Bank>().HasQueryFilter(filter => !filter.IsDeleted);
            modelBuilder.Entity<Bank>()
                .HasMany(p => p.BankDetails)
                .WithOne()
                .HasForeignKey(p => p.BankId);
            #endregion

            #region BankDetail ModelBuilder
            modelBuilder.Entity<BankDetail>().Property(p => p.BankDepositAmount).HasColumnType("money");
            modelBuilder.Entity<BankDetail>().Property(p => p.BankWithdrawalAmount).HasColumnType("money");
            modelBuilder.Entity<BankDetail>().HasQueryFilter(filter => !filter.IsDeleted);
            #endregion

            #region Customer ModelBuilder

            modelBuilder.Entity<Customer>().Property(p => p.DepositAmount).HasColumnType("money");
            modelBuilder.Entity<Customer>().Property(p => p.WithdrawalAmount).HasColumnType("money");
            modelBuilder.Entity<Customer>().Property(p => p.Type)
                .HasConversion(type => type.Value, value => CustomerTypeEnum.FromValue(value));
            modelBuilder.Entity<Customer>().HasQueryFilter(filter => !filter.IsDeleted);

            #endregion

        }

    }
}
