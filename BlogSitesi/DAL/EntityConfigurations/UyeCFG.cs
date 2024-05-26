using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogSitesi.Models;

namespace BlogSitesi.DAL.EntityConfigurations
{
    public class UyeCFG : IEntityTypeConfiguration<Uye>
    {
        public void Configure(EntityTypeBuilder<Uye> builder)
        {
            Uye uye = new Uye
            {
                Id = 1,
                Ad = "Bora",
                Soyad = "Yildirim",
                UserName = "Bora@gmail.com",
                NormalizedUserName = "BORA@GMAIL.COM",
                Email = "Bora@gmail.com",
                NormalizedEmail = "BORA@GMAIL.COM",
                Adres = "Uskudar",
                EmailConfirmed = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString()
            };


            PasswordHasher<Uye> passwordHasher = new PasswordHasher<Uye>();
            uye.PasswordHash = passwordHasher.HashPassword(uye, "BoraAdmin_123");

            builder.HasData(uye);

        }
    }
}
