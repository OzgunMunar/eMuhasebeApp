using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eMuhasebeServer.Infrastructure.Context
{
    internal sealed class CompanyDbContext : DbContext, IUnitOfWorkCompany
    {
        private string connectionString = string.Empty;

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
