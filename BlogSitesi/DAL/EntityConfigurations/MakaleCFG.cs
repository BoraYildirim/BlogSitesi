using BlogSitesi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSitesi.DAL.EntityConfigurations
{
    public class MakaleCFG : IEntityTypeConfiguration<Makale>
    {
        public void Configure(EntityTypeBuilder<Makale> builder)
        {
           builder.Property(x=>x.Baslık).HasColumnType("nvarchar").HasMaxLength(50);
        }
    }
}
