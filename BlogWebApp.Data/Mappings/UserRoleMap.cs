using BlogWebApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogWebApp.Data.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> b)
        {
            // Primary key
            b.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            b.ToTable("AspNetUserRoles");

            b.HasData(new AppUserRole {
            UserId= Guid.Parse("310A86E9-CAF7-4ACD-82FF-668A652F1439"),
            RoleId= Guid.Parse("37A73CDF-6F71-4366-9FB2-EAF8F03D1648")
            },
            new AppUserRole
            {
                UserId= Guid.Parse("607C334C-141F-4852-9C87-F37A70721C54"),
                RoleId= Guid.Parse("190E4C92-FF34-452B-9FD6-ED4B7CDDBE5F")
            }
            );

        }
    }
}
