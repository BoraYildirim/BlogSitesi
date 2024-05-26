using BlogSitesi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSitesi.DAL.EntityConfigurations
{
    public class KonuCFG : IEntityTypeConfiguration<Konu>
    {
        public void Configure(EntityTypeBuilder<Konu> builder)
        {
           builder.Property(x=>x.KonuBaslik).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
        }
    }
}
