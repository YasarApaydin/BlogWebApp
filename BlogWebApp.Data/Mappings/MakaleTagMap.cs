using BlogWebApp.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogWebApp.Data.Mappings
{
    public class MakaleTagMap : IEntityTypeConfiguration<MakaleTag>
    {
   

        public void Configure(EntityTypeBuilder<MakaleTag> builder)
        {

            builder.HasKey(mt => new { mt.MakaleId, mt.TagId });

            builder.HasOne(mt => mt.Makale)
                   .WithMany(m => m.MakaleTags)
                   .HasForeignKey(mt => mt.MakaleId);

            builder.HasOne(mt => mt.Tag)
                   .WithMany(t => t.MakaleTags)
                   .HasForeignKey(mt => mt.TagId);

            builder.ToTable("MakaleTags");
        }
    }
}
