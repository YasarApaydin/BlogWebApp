using BlogWebApp.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

namespace BlogWebApp.Data.Context
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,Guid,AppUserClaim,AppUserRole,AppUserLogin,AppRoleClaim,AppUserToken>
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
                      
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

           
            modelBuilder.Entity<Yorum>()
       .HasOne(y => y.Makale)
       .WithMany(m => m.Yorums)
       .HasForeignKey(y => y.MakaleId)
       .OnDelete(DeleteBehavior.Cascade);

         
            modelBuilder.Entity<Yorum>()
                .HasOne(y => y.User)
                .WithMany(u => u.Yorumlar)  
                .HasForeignKey(y => y.UserId)
                .OnDelete(DeleteBehavior.Restrict);

  
        }
    }
}
