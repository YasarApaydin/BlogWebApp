using BlogWebApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogWebApp.Data.Mappings
{
    public class ResimMap : IEntityTypeConfiguration<Resim>
    {
        public void Configure(EntityTypeBuilder<Resim> builder)
        {
            builder.HasData(new Resim
            {
                Id = Guid.Parse("84C3B69D-A397-4E09-874F-BD4407FCE6A0"),
                DosyaYolu = "https://ibb.co/yBPCxgqc",
                OlusturanKullanici = "Yasar",
           
                IsDeleted = false
            });
        }
    }
}
