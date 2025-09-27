using BlogWebApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogWebApp.Data.Mappings
{
    public class KategoriMap : IEntityTypeConfiguration<Kategori>
    {
        public void Configure(EntityTypeBuilder<Kategori> builder)
        {
            builder.HasData(new Kategori
            {
                Id=Guid.Parse("F443F2E5-FC5C-4556-A44B-227986CDEB54"),
                Ad="Bilimsel",
                OlusturanKullanici="Yasar",
                IsDeleted = false
            },
            new Kategori
            {
                Id = Guid.Parse("D9CB316E-3047-498A-A568-A80280723DC7"),
                Ad = "Akademik",
                OlusturanKullanici = "Cemil",
                IsDeleted = false
            });
        }
    }
}
