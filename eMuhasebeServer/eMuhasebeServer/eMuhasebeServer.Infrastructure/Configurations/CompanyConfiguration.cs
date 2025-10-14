using eMuhasebeServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eMuhasebeServer.Infrastructure.Configurations
{
    internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {

            builder.HasQueryFilter(x => !x.IsDeleted);
            builder.Property(x => x.TaxNumber).HasColumnType("varchar(11)");

            // Database record'u için options ayarlıyoruz.
            builder.OwnsOne(p => p.Database, databaseBuilder =>
            {

                databaseBuilder.Property(property => property.Server).HasColumnName("Server");
                databaseBuilder.Property(property => property.DatabaseName).HasColumnName("DatabaseName");
                databaseBuilder.Property(property => property.UserId).HasColumnName("UserId");
                databaseBuilder.Property(property => property.Password).HasColumnName("Password");

            });

        }
    }
}
