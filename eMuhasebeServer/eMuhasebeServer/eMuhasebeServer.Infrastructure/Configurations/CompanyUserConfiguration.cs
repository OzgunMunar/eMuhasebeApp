using eMuhasebeServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eMuhasebeServer.Infrastructure.Configurations
{
    internal class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
    {
        public void Configure(EntityTypeBuilder<CompanyUser> builder)
        {
            // Composite Key.
            builder.HasKey(x => new { x.AppUserId, x.CompanyId });
            builder.HasQueryFilter(filter => !filter.Company!.IsDeleted);

        }
    }
}
