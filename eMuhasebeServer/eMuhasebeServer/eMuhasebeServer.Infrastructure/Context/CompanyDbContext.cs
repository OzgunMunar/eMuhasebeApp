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

        private string connectionString = string.Empty;
        public DbSet<CashRegister> CashRegisters { get; set; }
        public DbSet<CashRegisterDetail> CashRegisterDetails { get; set; }
        public DbSet<Bank> Banks { get; set; }
        

        public CompanyDbContext(Company company)
        {
            CreateConnectionStringWithCompany(company);
        }

        // HttpContextAccessor Api'den gelen request'i yakalar.
        public CompanyDbContext(IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            CreateConnectionString(httpContextAccessor, applicationDbContext);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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

            #region CashRegisterDetailModelBuilder

            modelBuilder.Entity<CashRegisterDetail>().Property(p => p.CashDepositAmount).HasColumnType("money");
            modelBuilder.Entity<CashRegisterDetail>().Property(p => p.CashWithdrawalAmount).HasColumnType("money");
            modelBuilder.Entity<CashRegisterDetail>().HasQueryFilter(filter => !filter.IsDeleted);

            #endregion

            #region BankModelBuilder

            modelBuilder.Entity<Bank>().Property(p => p.BankDepositAmount).HasColumnType("money");
            modelBuilder.Entity<Bank>().Property(p => p.BankWithdrawalAmount).HasColumnType("money");
            modelBuilder.Entity<Bank>().Property(p => p.CurrencyType)
                .HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
            modelBuilder.Entity<Bank>().HasQueryFilter(filter => !filter.IsDeleted);

            #endregion

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

    }
}
