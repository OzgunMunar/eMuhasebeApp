using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Infrastructure.Context;
using GenericRepository;
using eMuhasebeServer.Domain.Repositories;

namespace eMuhasebeServer.Infrastructure.Repositories
{
    internal sealed class CompanyUserRepository : Repository<CompanyUser, ApplicationDbContext>, ICompanyUserRepository
    {
        public CompanyUserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
