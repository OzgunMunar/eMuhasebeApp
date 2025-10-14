using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace eMuhasebeServer.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        public void MigrateAll(List<Company> companies)
        {
            
            foreach (Company company in companies)
            {
                CompanyDbContext context = new(company);

                context.Database.EnsureCreated();

                context.Database.Migrate();
            }

        }
    }
}
