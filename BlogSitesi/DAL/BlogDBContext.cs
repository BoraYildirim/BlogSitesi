using BlogSitesi.Models;
using BlogSitesi.Models.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlogSitesi.DAL
{
    public class BlogDBContext : IdentityDbContext<Uye, Rol, int>
    {
        public BlogDBContext(DbContextOptions options) : base(options)
        {
        }

        protected BlogDBContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //User-Role tablosunda ilişkiyi yaz...
            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>() { UserId = 1, RoleId = 1 });
        }
        public DbSet<BlogSitesi.Models.ViewModels.Login_VM> Login_VM { get; set; } = default!;
        public DbSet<BlogSitesi.Models.ViewModels.Register_VM> Register_VM { get; set; } = default!;
        public DbSet<BlogSitesi.Models.Makale> Makale { get; set; } = default!;
        public DbSet<BlogSitesi.Models.Konu> Konu { get; set; } = default!;
    }
}
